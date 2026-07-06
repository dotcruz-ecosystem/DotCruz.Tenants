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
}
