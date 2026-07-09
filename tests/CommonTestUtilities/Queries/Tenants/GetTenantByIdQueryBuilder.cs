using Bogus;
using DotCruz.Tenants.Application.UseCases.Tenants.Queries.GetTenantById;

namespace CommonTestUtilities.Queries.Tenants;

public class GetTenantByIdQueryBuilder
{
    public static GetTenantByIdQuery Build(Guid? id = null)
    {
        return new GetTenantByIdQuery(id ?? Guid.NewGuid());
    }
}
