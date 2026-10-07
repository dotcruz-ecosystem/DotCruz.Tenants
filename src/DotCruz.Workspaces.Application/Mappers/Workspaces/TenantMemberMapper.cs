using DotCruz.Workspaces.Application.DTOs.Workspaces;
using DotCruz.Workspaces.Domain.Entities.Workspaces;

namespace DotCruz.Workspaces.Application.Mappers.Workspaces;

public static class WorkspaceMemberMapper
{
    public static WorkspaceMemberDto ToDto(this WorkspaceMember member)
    {
        return new WorkspaceMemberDto(
            member.Id,
            member.WorkspaceId,
            member.UserId,
            member.Email.Value,
            member.Role.ToString(),
            member.Status.ToString(),
            member.InvitedAt,
            member.JoinedAt
        );
    }
}
