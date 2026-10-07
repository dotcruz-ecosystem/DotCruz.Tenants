using DotCruz.Workspaces.Application.Abstractions.Data.Repositories.Base;
using DotCruz.Workspaces.Domain.Entities.Workspaces;

namespace DotCruz.Workspaces.Application.Abstractions.Data.Repositories.Workspaces;

public interface IWorkspaceWriteRepository : IBaseWriteRepository<Workspace>
{
    Task<Workspace?> GetByIdWithMembersToUpdateAsync(Guid id, CancellationToken cancellationToken);
}
