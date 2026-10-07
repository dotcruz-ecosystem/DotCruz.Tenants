using DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetUserWorkspaces;

namespace CommonTestUtilities.Queries.Workspaces;

public class GetUserWorkspacesQueryBuilder
{
    public static GetUserWorkspacesQuery Build(Guid? userId = null, bool isUserIdEmpty = false)
    {
        return new GetUserWorkspacesQuery(isUserIdEmpty ? Guid.Empty : (userId ?? Guid.NewGuid()));
    }
}
