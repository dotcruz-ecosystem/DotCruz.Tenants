using DotCruz.Tenants.Domain.Enums.Tenants;

namespace DotCruz.Tenants.Application.DTOs.Tenants;

public sealed record TenantSubscriptionDto(
    PlanType Plan,
    DateTimeOffset StartDate,
    DateTimeOffset? EndDate,
    DateTimeOffset? TrialEndDate,
    int MaxUsers,
    int MaxEmailsPerMonth
);
