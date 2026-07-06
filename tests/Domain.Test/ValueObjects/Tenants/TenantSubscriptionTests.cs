using CommonTestUtilities.ValueObjects.Tenants;
using CommonTestUtilities.ValueObjects.Temporal;
using DotCruz.Tenants.Domain.Enums.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using DotCruz.Tenants.Domain.ValueObjects.Temporal;
using DotCruz.Tenants.Domain.ValueObjects.Tenants;

namespace Domain.Test.ValueObjects.Tenants;

public class TenantSubscriptionTests
{
    [Fact]
    public void Success_Free()
    {
        var subscription = TenantSubscriptionBuilder.Build(PlanType.Free);

        Assert.NotNull(subscription);
        Assert.Equal(PlanType.Free, subscription.Plan);
        Assert.Null(subscription.TrialEndDate);
        Assert.Equal(ResourceLimits.CreateDefaultFree(), subscription.Limits);
    }

    [Fact]
    public void Success_Trial()
    {
        var subscription = TenantSubscriptionBuilder.Build(PlanType.Trial);

        Assert.NotNull(subscription);
        Assert.Equal(PlanType.Trial, subscription.Plan);
        Assert.NotNull(subscription.TrialEndDate);
        Assert.Equal(ResourceLimits.CreateDefaultTrial(), subscription.Limits);
    }

    [Fact]
    public void Success_Pro()
    {
        var subscription = TenantSubscriptionBuilder.Build(PlanType.Pro);

        Assert.NotNull(subscription);
        Assert.Equal(PlanType.Pro, subscription.Plan);
        Assert.Null(subscription.TrialEndDate);
        Assert.Equal(ResourceLimits.CreateDefaultPro(), subscription.Limits);
    }

    [Fact]
    public void Success_Enterprise()
    {
        var subscription = TenantSubscriptionBuilder.Build(PlanType.Enterprise);

        Assert.NotNull(subscription);
        Assert.Equal(PlanType.Enterprise, subscription.Plan);
        Assert.Null(subscription.TrialEndDate);
        Assert.Equal(ResourceLimits.CreateDefaultEnterprise(), subscription.Limits);
    }

    [Fact]
    public void Success_CreateTrial_Factory()
    {
        var subscription = TenantSubscription.CreateTrial(14);

        Assert.NotNull(subscription);
        Assert.Equal(PlanType.Trial, subscription.Plan);
        Assert.NotNull(subscription.TrialEndDate);
        Assert.Equal(subscription.Duration.End, subscription.TrialEndDate);
        Assert.Equal(ResourceLimits.CreateDefaultTrial(), subscription.Limits);
    }

    [Fact]
    public void Success_CreateFree_Factory()
    {
        var subscription = TenantSubscription.CreateFree();

        Assert.NotNull(subscription);
        Assert.Equal(PlanType.Free, subscription.Plan);
        Assert.Null(subscription.TrialEndDate);
        Assert.Equal(ResourceLimits.CreateDefaultFree(), subscription.Limits);
    }

    [Fact]
    public void Success_Upgrade_To_Pro()
    {
        var original = TenantSubscription.CreateFree();
        var upgraded = original.Upgrade(PlanType.Pro, 30);

        Assert.NotNull(upgraded);
        Assert.Equal(PlanType.Pro, upgraded.Plan);
        Assert.Null(upgraded.TrialEndDate);
        Assert.NotNull(upgraded.EndDate);
        Assert.Equal(ResourceLimits.CreateDefaultPro(), upgraded.Limits);
    }

    [Fact]
    public void Success_Upgrade_To_Enterprise_Has_No_EndDate()
    {
        var original = TenantSubscription.CreateFree();
        var upgraded = original.Upgrade(PlanType.Enterprise);

        Assert.NotNull(upgraded);
        Assert.Equal(PlanType.Enterprise, upgraded.Plan);
        Assert.Null(upgraded.TrialEndDate);
        Assert.Null(upgraded.EndDate);
        Assert.Equal(ResourceLimits.CreateDefaultEnterprise(), upgraded.Limits);
    }

    [Fact]
    public void Error_Trial_Needs_TrialEndDate()
    {
        static TenantSubscription act() => TenantSubscriptionBuilder.Build(PlanType.Trial, isTrialEndDateNull: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.SUBSCRIPTION_TRIAL_END_DATE_REQUIRED, exceptionMessage);
    }

    [Fact]
    public void Error_Subscription_Needs_Duration()
    {
        static TenantSubscription act() => TenantSubscriptionBuilder.Build(PlanType.Free, isDurationNull: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.DURATION_REQUIRED, exceptionMessage);
    }

    [Fact]
    public void Success_IsTrialExpired()
    {
        var now = DateTimeOffset.UtcNow;
        var duration = new DateTimePeriod(now.AddDays(-30), now.AddDays(-16));
        
        var expiredTrial = new TenantSubscription(PlanType.Trial, duration, now.AddDays(-16));
        Assert.True(expiredTrial.IsTrialExpired);

        var activeTrial = new TenantSubscription(PlanType.Trial, new DateTimePeriod(now, now.AddDays(14)), now.AddDays(14));
        Assert.False(activeTrial.IsTrialExpired);
    }
}
