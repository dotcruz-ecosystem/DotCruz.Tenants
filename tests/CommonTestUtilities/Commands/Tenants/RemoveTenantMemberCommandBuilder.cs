using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.RemoveWorkspaceMember;

namespace CommonTestUtilities.Commands.Tenants;

public class RemoveWorkspaceMemberCommandBuilder
{
    public static RemoveWorkspaceMemberCommand Build(
        Guid? WorkspaceId = null,
        Guid? memberId = null,
        bool isWorkspaceIdEmpty = false,
        bool isMemberIdEmpty = false)
    {
        return new RemoveWorkspaceMemberCommand(
            isWorkspaceIdEmpty ? Guid.Empty : (WorkspaceId ?? Guid.NewGuid()),
            isMemberIdEmpty ? Guid.Empty : (memberId ?? Guid.NewGuid())
        );
    }
}
