using CommonTestUtilities.Entities;
using CommonTestUtilities.ValueObjects.Tenants;
using DotCruz.Tenants.Domain.Entities.Tenants;
using DotCruz.Tenants.Domain.Enums.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;

namespace Domain.Test.Entities;

public class TenantTests
{
    [Fact]
    public void Success()
    {
        var tenant = new TenantBuilder().Build();
        
        Assert.NotNull(tenant);
        Assert.Equal(TenantStatus.PendingProvisioning, tenant.Status);
    }

    [Fact]
    public void Success_Activate()
    {
        var tenant = new TenantBuilder().Activate().Build();

        Assert.Equal(TenantStatus.Active, tenant.Status);
        Assert.Null(tenant.SuspensionReason);
    }

    [Fact]
    public void Success_Activate_TrialSubscription_ShouldBe_Trialing()
    {
        var trialSubscription = TenantSubscriptionBuilder.Build(PlanType.Trial);
        var tenant = new TenantBuilder(trialSubscription).Activate().Build();

        Assert.Equal(TenantStatus.Trialing, tenant.Status);
        Assert.Null(tenant.SuspensionReason);
    }

    [Fact]
    public void Success_Activate_SuspendedTenant_ClearsSuspensionReason()
    {
        var tenant = new TenantBuilder()
            .Activate()
            .Suspend()
            .Activate()
            .Build();

        Assert.Equal(TenantStatus.Active, tenant.Status);
        Assert.Null(tenant.SuspensionReason);
    }

    [Fact]
    public void Error_Activate_When_Tenant_Terminated()
    {
        static Tenant act() => new TenantBuilder()
            .Terminate()
            .Activate()
            .Build();

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.TENANT_TERMINATED_CANNOT_REACTIVATE, exceptionMessage);
    }

    [Fact]
    public void Success_Suspend()
    {
        var tenant = new TenantBuilder().Suspend().Build();

        Assert.Equal(TenantStatus.Suspended, tenant.Status);
        Assert.NotNull(tenant.SuspensionReason);
    }

    [Fact]
    public void Error_Suspension_Reason_Empty_On_Suspend()
    {
        static Tenant act() => new TenantBuilder()
            .Suspend(isSuspensionReasonNull: true)
            .Build();

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.SUSPENSION_REASON_EMPTY, exceptionMessage);
    }

    [Fact]
    public void Error_Terminated_On_Suspend()
    {
        static Tenant act() => new TenantBuilder()
            .Terminate()
            .Suspend()
            .Build();

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.TENANT_TERMINATED_CANNOT_SUSPEND, exceptionMessage);
    }

    [Fact]
    public void Success_Deactivate()
    {
        var tenant = new TenantBuilder().Deactivate().Build();
        Assert.Equal(TenantStatus.Inactive, tenant.Status);
    }

    [Fact]
    public void Error_Terminated_On_Deactivate()
    {
        static Tenant act() => new TenantBuilder()
            .Terminate()
            .Deactivate()
            .Build();

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.TENANT_TERMINATED_CANNOT_DEACTIVATE, exceptionMessage);
    }

    [Fact]
    public void Success_Terminate()
    {
        var tenant = new TenantBuilder().Terminate().Build();

        Assert.Equal(TenantStatus.Terminated, tenant.Status);
        Assert.NotNull(tenant.DeletedAt);
    }

    [Fact]
    public void Success_Update_Subscription()
    {
        var tenant = new TenantBuilder().Activate().UpdateSubscription().Build();
        
        Assert.Equal(TenantStatus.Active, tenant.Status);
    }

    [Fact]
    public void Success_UpdateSubscription_Transition_Trialing_To_Active()
    {
        var trialSubscription = TenantSubscriptionBuilder.Build(PlanType.Trial);
        var proSubscription = TenantSubscriptionBuilder.Build(PlanType.Pro);

        var tenant = new TenantBuilder(trialSubscription)
            .Activate()
            .UpdateSubscription(proSubscription)
            .Build();

        Assert.Equal(TenantStatus.Active, tenant.Status);
    }

    [Fact]
    public void Success_UpdateSubscription_DoesNotChangeStatus_When_PendingProvisioning()
    {
        var proSubscription = TenantSubscriptionBuilder.Build(PlanType.Pro);
        
        var tenant = new TenantBuilder()
            .UpdateSubscription(proSubscription)
            .Build();

        Assert.Equal(TenantStatus.PendingProvisioning, tenant.Status);
    }

    [Fact]
    public void Error_Update_Subscription_Null()
    {
        static Tenant act() => new TenantBuilder()
            .UpdateSubscription(isSubscriptionNull: true)
            .Build();

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.SUBSCRIPTION_REQUIRED, exceptionMessage);
    }

    [Fact]
    public void Success_Update_Contact()
    {
        var tenant = new TenantBuilder().UpdateContact().Build();
        Assert.NotNull(tenant.Contact);
    }

    [Fact]
    public void Error_Update_Contact_Null()
    {
        static Tenant act() => new TenantBuilder()
            .UpdateContact(isContactNull: true)
            .Build();

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.CONTACT_REQUIRED, exceptionMessage);
    }

    [Fact]
    public void Success_Update_Address()
    {
        var tenant = new TenantBuilder().UpdateAddress().Build();
        Assert.NotNull(tenant.Address);
    }

    [Fact]
    public void Error_Update_Address_Null()
    {
        static Tenant act() => new TenantBuilder()
            .UpdateAddress(isAddressNull: true)
            .Build();

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.ADDRESS_REQUIRED, exceptionMessage);
    }

    [Fact]
    public void Success_Mark_Past_Due()
    {
        var tenant = new TenantBuilder().Activate().MarkPastDue().Build();

        Assert.Equal(TenantStatus.PastDue, tenant.Status);
    }

    [Fact]
    public void Error_Mark_Past_Due_When_Invalid_Status()
    {
        static Tenant act() => new TenantBuilder()
            .Terminate()
            .MarkPastDue()
            .Build();

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.TENANT_PAST_DUE_INVALID_STATUS, exceptionMessage);
    }

    [Fact]
    public void Success_AddMember()
    {
        var tenant = new TenantBuilder().Activate().Build();
        var email = CommonTestUtilities.ValueObjects.Communication.EmailBuilder.Build();

        var member = tenant.AddMember(email, TenantRole.Member);

        Assert.NotNull(member);
        Assert.Contains(member, tenant.Members);
        Assert.Equal(email.Value, member.Email.Value);
    }

    [Fact]
    public void Error_AddMember_When_Tenant_Terminated()
    {
        var tenant = new TenantBuilder().Terminate().Build();
        var email = CommonTestUtilities.ValueObjects.Communication.EmailBuilder.Build();

        var exception = Assert.Throws<ErrorOnValidationException>(() =>
            tenant.AddMember(email, TenantRole.Member));

        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.TENANT_INACTIVE_CANNOT_ADD_MEMBER, message);
    }

    [Fact]
    public void Error_AddMember_When_Tenant_Deactivated()
    {
        var tenant = new TenantBuilder().Deactivate().Build();
        var email = CommonTestUtilities.ValueObjects.Communication.EmailBuilder.Build();

        var exception = Assert.Throws<ErrorOnValidationException>(() =>
            tenant.AddMember(email, TenantRole.Member));

        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.TENANT_INACTIVE_CANNOT_ADD_MEMBER, message);
    }

    [Fact]
    public void Error_AddMember_When_Email_Already_Exists()
    {
        var email = CommonTestUtilities.ValueObjects.Communication.EmailBuilder.Build();
        var tenant = new TenantBuilder()
            .Activate()
            .WithMember(email)
            .Build();

        var exception = Assert.Throws<ErrorOnValidationException>(() =>
            tenant.AddMember(email, TenantRole.Admin));

        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.MEMBER_ALREADY_EXISTS, message);
    }

    [Fact]
    public void Error_AddMember_When_MaxUsers_Limit_Reached()
    {
        // Free plan default has MaxUsers = 1
        var freeSubscription = CommonTestUtilities.ValueObjects.Tenants.TenantSubscriptionBuilder.Build(PlanType.Free);
        var email1 = CommonTestUtilities.ValueObjects.Communication.EmailBuilder.Build();
        var email2 = CommonTestUtilities.ValueObjects.Communication.EmailBuilder.Build();

        var tenant = new TenantBuilder(freeSubscription)
            .Activate()
            .WithMember(email1)
            .Build();

        var exception = Assert.Throws<ErrorOnValidationException>(() =>
            tenant.AddMember(email2, TenantRole.Member));

        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.MAX_USERS_REACHED, message);
    }

    [Fact]
    public void Success_RemoveMember()
    {
        var email = CommonTestUtilities.ValueObjects.Communication.EmailBuilder.Build();
        var tenant = new TenantBuilder().Activate().WithMember(email).Build();
        var member = Assert.Single(tenant.Members);

        tenant.RemoveMember(member.Id);

        Assert.NotNull(member.DeletedAt);
    }

    [Fact]
    public void Error_RemoveMember_When_NotFound()
    {
        var tenant = new TenantBuilder().Activate().Build();

        var exception = Assert.Throws<NotFoundException>(() =>
            tenant.RemoveMember(Guid.NewGuid()));

        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.MEMBER_NOT_FOUND, message);
    }

    [Fact]
    public void Error_RemoveMember_When_Last_Owner()
    {
        var email = CommonTestUtilities.ValueObjects.Communication.EmailBuilder.Build();
        var tenant = new TenantBuilder().Activate().WithOwnerMember(email).Build();
        var owner = Assert.Single(tenant.Members);

        var exception = Assert.Throws<ErrorOnValidationException>(() =>
            tenant.RemoveMember(owner.Id));

        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.CANNOT_REMOVE_LAST_OWNER, message);
    }

    [Fact]
    public void Success_RemoveMember_When_Multiple_Owners()
    {
        var email1 = CommonTestUtilities.ValueObjects.Communication.EmailBuilder.Build();
        var email2 = CommonTestUtilities.ValueObjects.Communication.EmailBuilder.Build();

        var proSubscription = CommonTestUtilities.ValueObjects.Tenants.TenantSubscriptionBuilder.Build(PlanType.Pro);
        var tenant = new TenantBuilder(proSubscription)
            .Activate()
            .WithOwnerMember(email1)
            .WithOwnerMember(email2)
            .Build();

        var owner1 = tenant.Members.First(m => m.Email.Value == email1.Value);

        tenant.RemoveMember(owner1.Id);

        Assert.NotNull(owner1.DeletedAt);
    }

    [Fact]
    public void Success_CreateWorkspace()
    {
        var name = CommonTestUtilities.ValueObjects.Identity.NameBuilder.Build();
        var slug = CommonTestUtilities.ValueObjects.Tenants.TenantSlugBuilder.Build();
        var ownerEmail = CommonTestUtilities.ValueObjects.Communication.EmailBuilder.Build();
        var ownerUserId = Guid.NewGuid();

        var tenant = Tenant.CreateWorkspace(name, slug, TenantType.Personal, ownerEmail: ownerEmail, ownerUserId: ownerUserId);

        Assert.NotNull(tenant);
        Assert.Equal(name.Value, tenant.Name.Value);
        Assert.Equal(slug.Value, tenant.Slug.Value);
        Assert.Equal(TenantType.Personal, tenant.Type);
        Assert.Equal(TenantStatus.Active, tenant.Status);
        Assert.Null(tenant.Document);
        Assert.Null(tenant.Contact);
        Assert.Null(tenant.Address);
        var member = Assert.Single(tenant.Members);
        Assert.Equal(ownerEmail.Value, member.Email.Value);
        Assert.Equal(ownerUserId, member.UserId);
        Assert.Equal(TenantRole.Owner, member.Role);
        Assert.Equal(MemberStatus.Active, member.Status);
    }
}
