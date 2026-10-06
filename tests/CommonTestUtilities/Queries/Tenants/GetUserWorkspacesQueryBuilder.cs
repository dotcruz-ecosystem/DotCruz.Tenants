using DotCruz.Tenants.Application.UseCases.Tenants.Queries.GetUserWorkspaces;

namespace CommonTestUtilities.Queries.Tenants;

public class GetUserWorkspacesQueryBuilder
{
    public static GetUserWorkspacesQuery Build(Guid? userId = null, bool isUserIdEmpty = false)
    {
        return new GetUserWorkspacesQuery(isUserIdEmpty ? Guid.Empty : (userId ?? Guid.NewGuid()));
    }
}
