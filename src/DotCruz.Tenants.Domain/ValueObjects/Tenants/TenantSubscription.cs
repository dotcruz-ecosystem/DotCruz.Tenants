using DotCruz.Tenants.Domain.Enums.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using DotCruz.Tenants.Domain.ValueObjects.Temporal;

namespace DotCruz.Tenants.Domain.ValueObjects.Tenants;

public record class TenantSubscription
{
    public PlanType Plan { get; init; }
    public DateTimePeriod Duration { get; init; } = null!;
    public DateTimeOffset? TrialEndDate { get; init; }
    public ResourceLimits Limits { get; init; } = null!;

    public DateTimeOffset StartDate => Duration.Start;
    public DateTimeOffset? EndDate => Duration.End;

    private TenantSubscription() { }

    public TenantSubscription(
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

    public static TenantSubscription CreateTrial(int durationDays = 14)
    {
        var now = DateTimeOffset.UtcNow;
        var end = now.AddDays(durationDays);
        var duration = new DateTimePeriod(now, end);
        return new TenantSubscription(
            PlanType.Trial, 
            duration, 
            end, 
            ResourceLimits.CreateDefaultTrial());
    }

    public static TenantSubscription CreateFree()
    {
        return new TenantSubscription(
            PlanType.Free, 
            new DateTimePeriod(DateTimeOffset.UtcNow), 
            limits: ResourceLimits.CreateDefaultFree());
    }

    public TenantSubscription Upgrade(PlanType newPlan, int durationDays = 30)
    {
        var now = DateTimeOffset.UtcNow;
        var end = newPlan == PlanType.Enterprise ? (DateTimeOffset?)null : now.AddDays(durationDays);
        var duration = new DateTimePeriod(now, end);
        var limits = GetDefaultLimits(newPlan);
        
        return new TenantSubscription(newPlan, duration, null, limits);
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
