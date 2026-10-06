namespace DotCruz.Tenants.Application.DTOs.Tenants;

public sealed record TenantMemberDto(
    Guid Id,
    Guid TenantId,
    Guid? UserId,
    string Email,
    string Role,
    string Status,
    DateTimeOffset InvitedAt,
    DateTimeOffset? JoinedAt
);
