using DotCruz.Workspaces.Application.Abstractions.Data.Repositories.Workspaces;
using DotCruz.Workspaces.Domain.Entities.Workspaces;
using Moq;

namespace CommonTestUtilities.Data.Repositories.Workspaces;

public class WorkspaceWriteRepositoryBuilder
{
    private readonly Mock<IWorkspaceWriteRepository> _repository = new();

    public void GetByIdToUpdate(Workspace Workspace, CancellationToken cancellationToken)
    {
        _repository.Setup(x => x.GetByIdToUpdateAsync(Workspace.Id, cancellationToken))
            .ReturnsAsync(Workspace);
    }

    public void GetByIdWithMembersToUpdate(Workspace Workspace, CancellationToken cancellationToken)
    {
        _repository.Setup(x => x.GetByIdWithMembersToUpdateAsync(Workspace.Id, cancellationToken))
            .ReturnsAsync(Workspace);
    }

    public Mock<IWorkspaceWriteRepository> Mock => _repository;
    public IWorkspaceWriteRepository Build() => _repository.Object;
}
