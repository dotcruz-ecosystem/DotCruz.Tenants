using CommonTestUtilities.Entities;
using CommonTestUtilities.ValueObjects.Workspaces;
using DotCruz.Workspaces.Domain.Entities.Workspaces;
using DotCruz.Workspaces.Domain.Enums.Workspaces;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;

namespace Domain.Test.Entities;

public class WorkspaceTests
{
    [Fact]
    public void Success()
    {
        var Workspace = new WorkspaceBuilder().Build();
        
        Assert.NotNull(Workspace);
        Assert.Equal(WorkspaceStatus.PendingProvisioning, Workspace.Status);
    }

    [Fact]
    public void Success_Activate()
    {
        var Workspace = new WorkspaceBuilder().Activate().Build();

        Assert.Equal(WorkspaceStatus.Active, Workspace.Status);
        Assert.Null(Workspace.SuspensionReason);
    }

    [Fact]
    public void Success_Activate_TrialSubscription_ShouldBe_Trialing()
    {
        var trialSubscription = WorkspaceSubscriptionBuilder.Build(PlanType.Trial);
        var Workspace = new WorkspaceBuilder(trialSubscription).Activate().Build();

        Assert.Equal(WorkspaceStatus.Trialing, Workspace.Status);
        Assert.Null(Workspace.SuspensionReason);
    }

    [Fact]
    public void Success_Activate_SuspendedTenant_ClearsSuspensionReason()
    {
        var Workspace = new WorkspaceBuilder()
            .Activate()
            .Suspend()
            .Activate()
            .Build();

        Assert.Equal(WorkspaceStatus.Active, Workspace.Status);
        Assert.Null(Workspace.SuspensionReason);
    }

    [Fact]
    public void Error_Activate_When_Tenant_Terminated()
    {
        static Workspace act() => new WorkspaceBuilder()
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
        var Workspace = new WorkspaceBuilder().Suspend().Build();

        Assert.Equal(WorkspaceStatus.Suspended, Workspace.Status);
        Assert.NotNull(Workspace.SuspensionReason);
    }

    [Fact]
    public void Error_Suspension_Reason_Empty_On_Suspend()
    {
        static Workspace act() => new WorkspaceBuilder()
            .Suspend(isSuspensionReasonNull: true)
            .Build();

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.SUSPENSION_REASON_EMPTY, exceptionMessage);
    }

    [Fact]
    public void Error_Terminated_On_Suspend()
    {
        static Workspace act() => new WorkspaceBuilder()
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
        var Workspace = new WorkspaceBuilder().Deactivate().Build();
        Assert.Equal(WorkspaceStatus.Inactive, Workspace.Status);
    }

    [Fact]
    public void Error_Terminated_On_Deactivate()
    {
        static Workspace act() => new WorkspaceBuilder()
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
        var Workspace = new WorkspaceBuilder().Terminate().Build();

        Assert.Equal(WorkspaceStatus.Terminated, Workspace.Status);
        Assert.NotNull(Workspace.DeletedAt);
    }

    [Fact]
    public void Success_Update_Subscription()
    {
        var Workspace = new WorkspaceBuilder().Activate().UpdateSubscription().Build();
        
        Assert.Equal(WorkspaceStatus.Active, Workspace.Status);
    }

    [Fact]
    public void Success_UpdateSubscription_Transition_Trialing_To_Active()
    {
        var trialSubscription = WorkspaceSubscriptionBuilder.Build(PlanType.Trial);
        var proSubscription = WorkspaceSubscriptionBuilder.Build(PlanType.Pro);

        var Workspace = new WorkspaceBuilder(trialSubscription)
            .Activate()
            .UpdateSubscription(proSubscription)
            .Build();

        Assert.Equal(WorkspaceStatus.Active, Workspace.Status);
    }

    [Fact]
    public void Success_UpdateSubscription_DoesNotChangeStatus_When_PendingProvisioning()
    {
        var proSubscription = WorkspaceSubscriptionBuilder.Build(PlanType.Pro);
        
        var Workspace = new WorkspaceBuilder()
            .UpdateSubscription(proSubscription)
            .Build();

        Assert.Equal(WorkspaceStatus.PendingProvisioning, Workspace.Status);
    }

    [Fact]
    public void Error_Update_Subscription_Null()
    {
        static Workspace act() => new WorkspaceBuilder()
            .UpdateSubscription(isSubscriptionNull: true)
            .Build();

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.SUBSCRIPTION_REQUIRED, exceptionMessage);
    }

    [Fact]
    public void Success_Update_Contact()
    {
        var Workspace = new WorkspaceBuilder().UpdateContact().Build();
        Assert.NotNull(Workspace.Contact);
    }

    [Fact]
    public void Error_Update_Contact_Null()
    {
        static Workspace act() => new WorkspaceBuilder()
            .UpdateContact(isContactNull: true)
            .Build();

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.CONTACT_REQUIRED, exceptionMessage);
    }

    [Fact]
    public void Success_Update_Address()
    {
        var Workspace = new WorkspaceBuilder().UpdateAddress().Build();
        Assert.NotNull(Workspace.Address);
    }

    [Fact]
    public void Error_Update_Address_Null()
    {
        static Workspace act() => new WorkspaceBuilder()
            .UpdateAddress(isAddressNull: true)
            .Build();

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.ADDRESS_REQUIRED, exceptionMessage);
    }

    [Fact]
    public void Success_Mark_Past_Due()
    {
        var Workspace = new WorkspaceBuilder().Activate().MarkPastDue().Build();

        Assert.Equal(WorkspaceStatus.PastDue, Workspace.Status);
    }

    [Fact]
    public void Error_Mark_Past_Due_When_Invalid_Status()
    {
        static Workspace act() => new WorkspaceBuilder()
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
        var Workspace = new WorkspaceBuilder().Activate().Build();
        var email = CommonTestUtilities.ValueObjects.Communication.EmailBuilder.Build();

        var member = Workspace.AddMember(email, WorkspaceRole.Member);

        Assert.NotNull(member);
        Assert.Contains(member, Workspace.Members);
        Assert.Equal(email.Value, member.Email.Value);
    }

    [Fact]
    public void Error_AddMember_When_Tenant_Terminated()
    {
        var Workspace = new WorkspaceBuilder().Terminate().Build();
        var email = CommonTestUtilities.ValueObjects.Communication.EmailBuilder.Build();

        var exception = Assert.Throws<ErrorOnValidationException>(() =>
            Workspace.AddMember(email, WorkspaceRole.Member));

        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.TENANT_INACTIVE_CANNOT_ADD_MEMBER, message);
    }

    [Fact]
    public void Error_AddMember_When_Tenant_Deactivated()
    {
        var Workspace = new WorkspaceBuilder().Deactivate().Build();
        var email = CommonTestUtilities.ValueObjects.Communication.EmailBuilder.Build();

        var exception = Assert.Throws<ErrorOnValidationException>(() =>
            Workspace.AddMember(email, WorkspaceRole.Member));

        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.TENANT_INACTIVE_CANNOT_ADD_MEMBER, message);
    }

    [Fact]
    public void Error_AddMember_When_Email_Already_Exists()
    {
        var email = CommonTestUtilities.ValueObjects.Communication.EmailBuilder.Build();
        var Workspace = new WorkspaceBuilder()
            .Activate()
            .WithMember(email)
            .Build();

        var exception = Assert.Throws<ErrorOnValidationException>(() =>
            Workspace.AddMember(email, WorkspaceRole.Admin));

        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.MEMBER_ALREADY_EXISTS, message);
    }

    [Fact]
    public void Error_AddMember_When_MaxUsers_Limit_Reached()
    {
        // Free plan default has MaxUsers = 1
        var freeSubscription = CommonTestUtilities.ValueObjects.Workspaces.WorkspaceSubscriptionBuilder.Build(PlanType.Free);
        var email1 = CommonTestUtilities.ValueObjects.Communication.EmailBuilder.Build();
        var email2 = CommonTestUtilities.ValueObjects.Communication.EmailBuilder.Build();

        var Workspace = new WorkspaceBuilder(freeSubscription)
            .Activate()
            .WithMember(email1)
            .Build();

        var exception = Assert.Throws<ErrorOnValidationException>(() =>
            Workspace.AddMember(email2, WorkspaceRole.Member));

        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.MAX_USERS_REACHED, message);
    }

    [Fact]
    public void Success_RemoveMember()
    {
        var email = CommonTestUtilities.ValueObjects.Communication.EmailBuilder.Build();
        var Workspace = new WorkspaceBuilder().Activate().WithMember(email).Build();
        var member = Assert.Single(Workspace.Members);

        Workspace.RemoveMember(member.Id);

        Assert.NotNull(member.DeletedAt);
    }

    [Fact]
    public void Error_RemoveMember_When_NotFound()
    {
        var Workspace = new WorkspaceBuilder().Activate().Build();

        var exception = Assert.Throws<NotFoundException>(() =>
            Workspace.RemoveMember(Guid.NewGuid()));

        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.MEMBER_NOT_FOUND, message);
    }

    [Fact]
    public void Error_RemoveMember_When_Last_Owner()
    {
        var email = CommonTestUtilities.ValueObjects.Communication.EmailBuilder.Build();
        var Workspace = new WorkspaceBuilder().Activate().WithOwnerMember(email).Build();
        var owner = Assert.Single(Workspace.Members);

        var exception = Assert.Throws<ErrorOnValidationException>(() =>
            Workspace.RemoveMember(owner.Id));

        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.CANNOT_REMOVE_LAST_OWNER, message);
    }

    [Fact]
    public void Success_RemoveMember_When_Multiple_Owners()
    {
        var email1 = CommonTestUtilities.ValueObjects.Communication.EmailBuilder.Build();
        var email2 = CommonTestUtilities.ValueObjects.Communication.EmailBuilder.Build();

        var proSubscription = CommonTestUtilities.ValueObjects.Workspaces.WorkspaceSubscriptionBuilder.Build(PlanType.Pro);
        var Workspace = new WorkspaceBuilder(proSubscription)
            .Activate()
            .WithOwnerMember(email1)
            .WithOwnerMember(email2)
            .Build();

        var owner1 = Workspace.Members.First(m => m.Email.Value == email1.Value);

        Workspace.RemoveMember(owner1.Id);

        Assert.NotNull(owner1.DeletedAt);
    }

    [Fact]
    public void Success_CreateWorkspace()
    {
        var name = CommonTestUtilities.ValueObjects.Identity.NameBuilder.Build();
        var slug = CommonTestUtilities.ValueObjects.Workspaces.WorkspaceSlugBuilder.Build();
        var ownerEmail = CommonTestUtilities.ValueObjects.Communication.EmailBuilder.Build();
        var ownerUserId = Guid.NewGuid();

        var workspace = Workspace.CreateWorkspace(name, slug, WorkspaceType.Personal, ownerEmail: ownerEmail, ownerUserId: ownerUserId);

        Assert.NotNull(workspace);
        Assert.Equal(name.Value, workspace.Name.Value);
        Assert.Equal(slug.Value, workspace.Slug.Value);
        Assert.Equal(WorkspaceType.Personal, workspace.Type);
        Assert.Equal(WorkspaceStatus.Active, workspace.Status);
        Assert.Null(workspace.Document);
        Assert.Null(workspace.Contact);
        Assert.Null(workspace.Address);
        var member = Assert.Single(workspace.Members);
        Assert.Equal(ownerEmail.Value, member.Email.Value);
        Assert.Equal(ownerUserId, member.UserId);
        Assert.Equal(WorkspaceRole.Owner, member.Role);
        Assert.Equal(MemberStatus.Active, member.Status);
    }
}
