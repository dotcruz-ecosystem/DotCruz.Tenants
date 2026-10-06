namespace DotCruz.Tenants.Application.DTOs.Tenants;

public sealed record AddTenantMemberRequest(
    string Email,
    string? Role = null,
    Guid? UserId = null
);
