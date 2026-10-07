using DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetWorkspaceMembers;

namespace CommonTestUtilities.Queries.Workspaces;

public class GetWorkspaceMembersQueryBuilder
{
    public static GetWorkspaceMembersQuery Build(Guid? WorkspaceId = null, bool isWorkspaceIdEmpty = false)
    {
        return new GetWorkspaceMembersQuery(isWorkspaceIdEmpty ? Guid.Empty : (WorkspaceId ?? Guid.NewGuid()));
    }
}
