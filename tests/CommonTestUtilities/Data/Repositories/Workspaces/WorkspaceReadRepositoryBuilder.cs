using DotCruz.Workspaces.Application.Abstractions.Data.Repositories.Workspaces;
using DotCruz.Workspaces.Domain.Entities.Workspaces;
using DotCruz.Workspaces.Domain.Enums.Workspaces;
using Moq;

namespace CommonTestUtilities.Data.Repositories.Workspaces;

public class WorkspaceReadRepositoryBuilder
{
    private readonly Mock<IWorkspaceReadRepository> _WorkspaceReadRepositoryMock = new();
    
    public void ExistsWithSlugAsync(string slug, CancellationToken cancellationToken)
    {
        _WorkspaceReadRepositoryMock
            .Setup(r => r.ExistsWithSlugAsync(slug, cancellationToken))
            .ReturnsAsync(true);
    }

    public void ExistsWithDocumentAsync(string documentNumber, CancellationToken cancellationToken)
    {
        _WorkspaceReadRepositoryMock
            .Setup(r => r.ExistsWithDocumentAsync(documentNumber, cancellationToken))
            .ReturnsAsync(true);
    }

    public void GetByIdAsync(Guid id, Workspace? Workspace, CancellationToken cancellationToken)
    {
        _WorkspaceReadRepositoryMock
            .Setup(r => r.GetByIdAsync(id, cancellationToken))
            .ReturnsAsync(Workspace);
    }

    public void GetBySlugAsync(string slug, Workspace? Workspace, CancellationToken cancellationToken)
    {
        _WorkspaceReadRepositoryMock
            .Setup(r => r.GetBySlugAsync(slug, cancellationToken))
            .ReturnsAsync(Workspace);
    }

    public void SearchAsync(
        int pageNumber,
        int pageSize,
        WorkspaceStatus? status,
        PlanType? plan,
        string? searchTerm,
        IReadOnlyCollection<Workspace> items,
        int totalCount,
        CancellationToken cancellationToken)
    {
        _WorkspaceReadRepositoryMock
            .Setup(r => r.SearchAsync(pageNumber, pageSize, status, plan, searchTerm, cancellationToken))
            .ReturnsAsync((items, totalCount));
    }

    public void GetMembersAsync(Guid WorkspaceId, IReadOnlyCollection<WorkspaceMember> members, CancellationToken cancellationToken)
    {
        _WorkspaceReadRepositoryMock
            .Setup(r => r.GetMembersAsync(WorkspaceId, cancellationToken))
            .ReturnsAsync(members);
    }

    public void GetUserWorkspacesAsync(Guid userId, IReadOnlyCollection<(Workspace Workspace, WorkspaceMember Member)> workspaces, CancellationToken cancellationToken)
    {
        _WorkspaceReadRepositoryMock
            .Setup(r => r.GetUserWorkspacesAsync(userId, cancellationToken))
            .ReturnsAsync(workspaces);
    }

    public void GetUserWorkspacesByEmailAsync(string email, IReadOnlyCollection<(Workspace Workspace, WorkspaceMember Member)> workspaces, CancellationToken cancellationToken)
    {
        _WorkspaceReadRepositoryMock
            .Setup(r => r.GetUserWorkspacesByEmailAsync(email, cancellationToken))
            .ReturnsAsync(workspaces);
    }

    public IWorkspaceReadRepository Build() => _WorkspaceReadRepositoryMock.Object;
}
