using CommonTestUtilities.ValueObjects.Communication;
using DotCruz.Workspaces.Domain.Entities.Workspaces;
using DotCruz.Workspaces.Domain.Enums.Workspaces;
using DotCruz.Workspaces.Domain.ValueObjects.Communication;

namespace CommonTestUtilities.Entities;

public class WorkspaceMemberBuilder
{
    public static WorkspaceMember Build(
        Guid? WorkspaceId = null,
        Email? email = null,
        WorkspaceRole role = WorkspaceRole.Member,
        Guid? userId = null,
        MemberStatus status = MemberStatus.Invited)
    {
        return WorkspaceMember.Create(
            WorkspaceId ?? Guid.NewGuid(),
            email ?? EmailBuilder.Build(),
            role,
            userId,
            status
        );
    }
}
