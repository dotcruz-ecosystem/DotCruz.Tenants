using DotCruz.Workspaces.Domain.Enums.Workspaces;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;
using DotCruz.Workspaces.Domain.ValueObjects.Temporal;

namespace DotCruz.Workspaces.Domain.ValueObjects.Workspaces;

public record class WorkspaceSubscription
{
    public PlanType Plan { get; init; }
    public DateTimePeriod Duration { get; init; } = null!;
    public DateTimeOffset? TrialEndDate { get; init; }
    public ResourceLimits Limits { get; init; } = null!;

    public DateTimeOffset StartDate => Duration.Start;
    public DateTimeOffset? EndDate => Duration.End;

    private WorkspaceSubscription() { }

    public WorkspaceSubscription(
        PlanType plan, 
        DateTimePeriod duration, 
        DateTimeOffset? trialEndDate = null,
        ResourceLimits? limits = null)
    {
        if (plan == PlanType.Trial && !trialEndDate.HasValue)
            throw new ErrorOnValidationException(ResourceMessagesException.SUBSCRIPTION_TRIAL_END_DATE_REQUIRED);

        Plan = plan;
        Duration = duration ?? throw new ErrorOnValidationException(ResourceMessagesException.DURATION_REQUIRED);
        TrialEndDate = trialEndDate;
        Limits = limits ?? GetDefaultLimits(plan);
    }

    public bool IsExpired => Duration.IsExpired;
    
    public bool IsTrialExpired => Plan == PlanType.Trial && TrialEndDate.HasValue && DateTimeOffset.UtcNow > TrialEndDate.Value;

    public static WorkspaceSubscription CreateTrial(int durationDays = 14)
    {
        var now = DateTimeOffset.UtcNow;
        var end = now.AddDays(durationDays);
        var duration = new DateTimePeriod(now, end);
        return new WorkspaceSubscription(
            PlanType.Trial, 
            duration, 
            end, 
            ResourceLimits.CreateDefaultTrial());
    }

    public static WorkspaceSubscription CreateFree()
    {
        return new WorkspaceSubscription(
            PlanType.Free, 
            new DateTimePeriod(DateTimeOffset.UtcNow), 
            limits: ResourceLimits.CreateDefaultFree());
    }

    public WorkspaceSubscription Upgrade(PlanType newPlan, int durationDays = 30)
    {
        var now = DateTimeOffset.UtcNow;
        var end = newPlan == PlanType.Enterprise ? (DateTimeOffset?)null : now.AddDays(durationDays);
        var duration = new DateTimePeriod(now, end);
        var limits = GetDefaultLimits(newPlan);
        
        return new WorkspaceSubscription(newPlan, duration, null, limits);
    }

    private static ResourceLimits GetDefaultLimits(PlanType plan) => plan switch
    {
        PlanType.Free => ResourceLimits.CreateDefaultFree(),
        PlanType.Trial => ResourceLimits.CreateDefaultTrial(),
        PlanType.Pro => ResourceLimits.CreateDefaultPro(),
        PlanType.Enterprise => ResourceLimits.CreateDefaultEnterprise(),
        _ => ResourceLimits.CreateDefaultFree()
    };
}
