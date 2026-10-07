namespace DotCruz.Workspaces.Application.Abstractions.Security;

public interface IWorkspaceScopedRequest
{
    Guid WorkspaceId { get; }
}
