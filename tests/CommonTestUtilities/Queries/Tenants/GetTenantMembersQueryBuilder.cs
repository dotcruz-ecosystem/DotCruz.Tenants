using DotCruz.Tenants.Application.UseCases.Tenants.Queries.GetTenantMembers;

namespace CommonTestUtilities.Queries.Tenants;

public class GetTenantMembersQueryBuilder
{
    public static GetTenantMembersQuery Build(Guid? tenantId = null, bool isTenantIdEmpty = false)
    {
        return new GetTenantMembersQuery(isTenantIdEmpty ? Guid.Empty : (tenantId ?? Guid.NewGuid()));
    }
}
