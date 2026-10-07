namespace DotCruz.Workspaces.Application.Abstractions.Services.CoreAuth.Requests;

public sealed record CreateWorkspaceAdminUserRequestDto
{
    private const int ADMIN_USER_TYPE = 2;

    public string Name { get; private set; }
    public string Email { get; private set; }
    public Guid WorkspaceId { get; private set; }
    public int Type { get; } = ADMIN_USER_TYPE;

    public CreateWorkspaceAdminUserRequestDto(string name, string email, Guid workspaceId)
    {
        Name = name;
        Email = email;
        WorkspaceId = workspaceId;
    }
}
