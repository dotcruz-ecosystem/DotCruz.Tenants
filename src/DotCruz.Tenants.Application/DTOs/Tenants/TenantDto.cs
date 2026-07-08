using DotCruz.Tenants.Application.DTOs.Fiscal;

namespace DotCruz.Tenants.Application.DTOs.Tenants;

public sealed record TenantDto(
    Guid Id,
    string Name,
    string Slug,
    FiscalDocumentDto TenantDocument,
    string Status,
    TenantContactDto TenantContact,
    TenantAddressDto TenantAddress,
    TenantSubscriptionDto TenantSubscription,
    string? SuspensionReason,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
);
