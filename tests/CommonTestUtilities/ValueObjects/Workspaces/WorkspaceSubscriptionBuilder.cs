using Bogus;
using CommonTestUtilities.ValueObjects.Temporal;
using DotCruz.Workspaces.Domain.Enums.Workspaces;
using DotCruz.Workspaces.Domain.ValueObjects.Temporal;
using DotCruz.Workspaces.Domain.ValueObjects.Workspaces;

namespace CommonTestUtilities.ValueObjects.Workspaces;

public class WorkspaceSubscriptionBuilder
{
    public static WorkspaceSubscription Build(
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

        return new WorkspaceSubscription(
            plan,
            duration!,
            trialEndDate,
            customLimits
        );
    }
}
