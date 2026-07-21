namespace DotCruz.Tenants.Application.DTOs.Tenants;

public sealed record TenantSummaryDto(
    Guid Id,
    string Name,
    string Slug,
    string Status,
    string Type,
    string Plan
);
