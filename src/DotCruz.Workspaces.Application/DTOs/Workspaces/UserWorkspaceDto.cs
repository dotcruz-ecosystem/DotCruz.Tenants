namespace DotCruz.Workspaces.Application.DTOs.Workspaces;

public sealed record UserWorkspaceDto(
    Guid WorkspaceId,
    string WorkspaceName,
    string WorkspaceSlug,
    string WorkspaceType,
    string WorkspaceStatus,
    string Role,
    string MemberStatus,
    string Plan
);
