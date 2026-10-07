namespace DotCruz.Workspaces.Application.DTOs.Workspaces;

public sealed record AddWorkspaceMemberRequest(
    string Email,
    string? Role = null,
    Guid? UserId = null
);
