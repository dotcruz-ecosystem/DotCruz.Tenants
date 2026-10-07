using DotCruz.Workspaces.Domain.Enums.Workspaces;

namespace DotCruz.Workspaces.Application.DTOs.Workspaces;

public sealed record WorkspaceSubscriptionDto(
    PlanType Plan,
    DateTimeOffset StartDate,
    DateTimeOffset? EndDate,
    DateTimeOffset? TrialEndDate,
    int MaxUsers,
    int MaxEmailsPerMonth
);
