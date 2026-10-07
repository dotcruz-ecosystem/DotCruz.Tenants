using DotCruz.Workspaces.Application.Abstractions.Data.Repositories.Base;
using DotCruz.Workspaces.Domain.Entities.Workspaces;
using DotCruz.Workspaces.Domain.Enums.Workspaces;

namespace DotCruz.Workspaces.Application.Abstractions.Data.Repositories.Workspaces;

public interface IWorkspaceReadRepository : IBaseReadRepository<Workspace>
{
    Task<bool> ExistsWithSlugAsync(string slug, CancellationToken cancellationToken);
    Task<bool> ExistsWithDocumentAsync(string documentNumber, CancellationToken cancellationToken);
    Task<Workspace?> GetBySlugAsync(string slug, CancellationToken cancellationToken);
    Task<(IReadOnlyCollection<Workspace> Items, int TotalCount)> SearchAsync(
        int pageNumber,
        int pageSize,
        WorkspaceStatus? status,
        PlanType? plan,
        string? searchTerm,
        CancellationToken cancellationToken);
    Task<IReadOnlyCollection<WorkspaceMember>> GetMembersAsync(Guid WorkspaceId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<(Workspace Workspace, WorkspaceMember Member)>> GetUserWorkspacesAsync(Guid userId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<(Workspace Workspace, WorkspaceMember Member)>> GetUserWorkspacesByEmailAsync(string email, CancellationToken cancellationToken);
}
