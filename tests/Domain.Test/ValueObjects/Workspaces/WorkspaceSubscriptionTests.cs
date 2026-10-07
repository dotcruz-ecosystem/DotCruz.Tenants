using CommonTestUtilities.ValueObjects.Workspaces;
using CommonTestUtilities.ValueObjects.Temporal;
using DotCruz.Workspaces.Domain.Enums.Workspaces;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;
using DotCruz.Workspaces.Domain.ValueObjects.Temporal;
using DotCruz.Workspaces.Domain.ValueObjects.Workspaces;

namespace Domain.Test.ValueObjects.Workspaces;

public class WorkspaceSubscriptionTests
{
    [Fact]
    public void Success_Free()
    {
        var subscription = WorkspaceSubscriptionBuilder.Build(PlanType.Free);

        Assert.NotNull(subscription);
        Assert.Equal(PlanType.Free, subscription.Plan);
        Assert.Null(subscription.TrialEndDate);
        Assert.Equal(ResourceLimits.CreateDefaultFree(), subscription.Limits);
    }

    [Fact]
    public void Success_Trial()
    {
        var subscription = WorkspaceSubscriptionBuilder.Build(PlanType.Trial);

        Assert.NotNull(subscription);
        Assert.Equal(PlanType.Trial, subscription.Plan);
        Assert.NotNull(subscription.TrialEndDate);
        Assert.Equal(ResourceLimits.CreateDefaultTrial(), subscription.Limits);
    }

    [Fact]
    public void Success_Pro()
    {
        var subscription = WorkspaceSubscriptionBuilder.Build(PlanType.Pro);

        Assert.NotNull(subscription);
        Assert.Equal(PlanType.Pro, subscription.Plan);
        Assert.Null(subscription.TrialEndDate);
        Assert.Equal(ResourceLimits.CreateDefaultPro(), subscription.Limits);
    }

    [Fact]
    public void Success_Enterprise()
    {
        var subscription = WorkspaceSubscriptionBuilder.Build(PlanType.Enterprise);

        Assert.NotNull(subscription);
        Assert.Equal(PlanType.Enterprise, subscription.Plan);
        Assert.Null(subscription.TrialEndDate);
        Assert.Equal(ResourceLimits.CreateDefaultEnterprise(), subscription.Limits);
    }

    [Fact]
    public void Success_CreateTrial_Factory()
    {
        var subscription = WorkspaceSubscription.CreateTrial(14);

        Assert.NotNull(subscription);
        Assert.Equal(PlanType.Trial, subscription.Plan);
        Assert.NotNull(subscription.TrialEndDate);
        Assert.Equal(subscription.Duration.End, subscription.TrialEndDate);
        Assert.Equal(ResourceLimits.CreateDefaultTrial(), subscription.Limits);
    }

    [Fact]
    public void Success_CreateFree_Factory()
    {
        var subscription = WorkspaceSubscription.CreateFree();

        Assert.NotNull(subscription);
        Assert.Equal(PlanType.Free, subscription.Plan);
        Assert.Null(subscription.TrialEndDate);
        Assert.Equal(ResourceLimits.CreateDefaultFree(), subscription.Limits);
    }

    [Fact]
    public void Success_Upgrade_To_Pro()
    {
        var original = WorkspaceSubscription.CreateFree();
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
        var original = WorkspaceSubscription.CreateFree();
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
        static WorkspaceSubscription act() => WorkspaceSubscriptionBuilder.Build(PlanType.Trial, isTrialEndDateNull: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.SUBSCRIPTION_TRIAL_END_DATE_REQUIRED, exceptionMessage);
    }

    [Fact]
    public void Error_Subscription_Needs_Duration()
    {
        static WorkspaceSubscription act() => WorkspaceSubscriptionBuilder.Build(PlanType.Free, isDurationNull: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.DURATION_REQUIRED, exceptionMessage);
    }

    [Fact]
    public void Success_IsTrialExpired()
    {
        var now = DateTimeOffset.UtcNow;
        var duration = new DateTimePeriod(now.AddDays(-30), now.AddDays(-16));
        
        var expiredTrial = new WorkspaceSubscription(PlanType.Trial, duration, now.AddDays(-16));
        Assert.True(expiredTrial.IsTrialExpired);

        var activeTrial = new WorkspaceSubscription(PlanType.Trial, new DateTimePeriod(now, now.AddDays(14)), now.AddDays(14));
        Assert.False(activeTrial.IsTrialExpired);
    }
}
