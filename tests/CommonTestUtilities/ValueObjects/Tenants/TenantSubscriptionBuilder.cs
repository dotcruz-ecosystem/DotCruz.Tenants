using Bogus;
using CommonTestUtilities.ValueObjects.Temporal;
using DotCruz.Tenants.Domain.Enums.Tenants;
using DotCruz.Tenants.Domain.ValueObjects.Temporal;
using DotCruz.Tenants.Domain.ValueObjects.Tenants;

namespace CommonTestUtilities.ValueObjects.Tenants;

public class TenantSubscriptionBuilder
{
    public static TenantSubscription Build(
        PlanType plan = PlanType.Free,
        bool isDurationNull = false,
        bool isTrialEndDateNull = false,
        ResourceLimits? customLimits = null)
    {
        DateTimePeriod? duration = isDurationNull ? null : DateTimePeriodBuilder.Build();

        DateTimeOffset? trialEndDate = null;
        if (plan == PlanType.Trial && !isTrialEndDateNull)
        {
            trialEndDate = DateTimeOffset.UtcNow.AddDays(14);
        }

        return new TenantSubscription(
            plan,
            duration!,
            trialEndDate,
            customLimits
        );
    }
}
