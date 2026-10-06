using DotCruz.Tenants.Application.UseCases.Tenants.Commands.RemoveTenantMember;

namespace CommonTestUtilities.Commands.Tenants;

public class RemoveTenantMemberCommandBuilder
{
    public static RemoveTenantMemberCommand Build(
        Guid? tenantId = null,
        Guid? memberId = null,
        bool isTenantIdEmpty = false,
        bool isMemberIdEmpty = false)
    {
        return new RemoveTenantMemberCommand(
            isTenantIdEmpty ? Guid.Empty : (tenantId ?? Guid.NewGuid()),
            isMemberIdEmpty ? Guid.Empty : (memberId ?? Guid.NewGuid())
        );
    }
}
