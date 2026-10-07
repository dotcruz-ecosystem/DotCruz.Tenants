using DotCruz.Workspaces.Application.DTOs.Workspaces;
using DotCruz.Workspaces.Domain.ValueObjects.Temporal;
using DotCruz.Workspaces.Domain.ValueObjects.Workspaces;

namespace DotCruz.Workspaces.Application.Mappers.Workspaces;

public static class WorkspaceSubscriptionMapper
{
    public static WorkspaceSubscriptionDto ToDto(this WorkspaceSubscription subscription)
    {
        return new WorkspaceSubscriptionDto(
            subscription.Plan,
            subscription.StartDate,
            subscription.EndDate,
            subscription.TrialEndDate,
            subscription.Limits.MaxUsers,
            subscription.Limits.MaxEmailsPerMonth
        );
    }

    public static WorkspaceSubscription ToDomain(this WorkspaceSubscriptionDto subscriptionDto)
    {
        var duration = new DateTimePeriod(subscriptionDto.StartDate, subscriptionDto.EndDate);
        var limits = new ResourceLimits(subscriptionDto.MaxUsers, subscriptionDto.MaxEmailsPerMonth);
        return new WorkspaceSubscription(
            subscriptionDto.Plan,
            duration,
            subscriptionDto.TrialEndDate,
            limits
        );
    }
}
