using Bogus;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetTenantById;

namespace CommonTestUtilities.Queries.Workspaces;

public class GetTenantByIdQueryBuilder
{
    public static GetTenantByIdQuery Build(Guid? id = null)
    {
        return new GetTenantByIdQuery(id ?? Guid.NewGuid());
    }
}
