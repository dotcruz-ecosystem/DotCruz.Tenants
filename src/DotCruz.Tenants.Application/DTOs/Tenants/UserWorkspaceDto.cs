namespace DotCruz.Tenants.Application.DTOs.Tenants;

public sealed record UserWorkspaceDto(
    Guid TenantId,
    string TenantName,
    string TenantSlug,
    string TenantType,
    string TenantStatus,
    string Role,
    string MemberStatus,
    string Plan
);
