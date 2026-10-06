using DotCruz.Tenants.Application.DTOs.Tenants;
using DotCruz.Tenants.Domain.Entities.Tenants;

namespace DotCruz.Tenants.Application.Mappers.Tenants;

public static class TenantMemberMapper
{
    public static TenantMemberDto ToDto(this TenantMember member)
    {
        return new TenantMemberDto(
            member.Id,
            member.TenantId,
            member.UserId,
            member.Email.Value,
            member.Role.ToString(),
            member.Status.ToString(),
            member.InvitedAt,
            member.JoinedAt
        );
    }
}
