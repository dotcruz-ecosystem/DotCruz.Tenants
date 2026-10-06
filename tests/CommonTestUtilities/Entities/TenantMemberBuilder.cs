using CommonTestUtilities.ValueObjects.Communication;
using DotCruz.Tenants.Domain.Entities.Tenants;
using DotCruz.Tenants.Domain.Enums.Tenants;
using DotCruz.Tenants.Domain.ValueObjects.Communication;

namespace CommonTestUtilities.Entities;

public class TenantMemberBuilder
{
    public static TenantMember Build(
        Guid? tenantId = null,
        Email? email = null,
        TenantRole role = TenantRole.Member,
        Guid? userId = null,
        MemberStatus status = MemberStatus.Invited)
    {
        return TenantMember.Create(
            tenantId ?? Guid.NewGuid(),
            email ?? EmailBuilder.Build(),
            role,
            userId,
            status
        );
    }
}
