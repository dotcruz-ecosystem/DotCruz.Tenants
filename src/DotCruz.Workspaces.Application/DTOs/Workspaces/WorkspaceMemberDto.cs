namespace DotCruz.Workspaces.Application.DTOs.Workspaces;

public sealed record WorkspaceMemberDto(
    Guid Id,
    Guid WorkspaceId,
    Guid? UserId,
    string Email,
    string Role,
    string Status,
    DateTimeOffset InvitedAt,
    DateTimeOffset? JoinedAt
);
