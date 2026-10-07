using DotCruz.Workspaces.Application.Abstractions.Data.Repositories.Workspaces;
using DotCruz.Workspaces.Domain.Entities.Workspaces;
using DotCruz.Workspaces.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace DotCruz.Workspaces.Infrastructure.Data.Repositories.Workspaces;

public class WorkspaceWriteRepository(WorkspaceDbContext context) 
    : BaseWriteRepository<Workspace>(context), IWorkspaceWriteRepository
{
    public Task<Workspace?> GetByIdWithMembersToUpdateAsync(Guid id, CancellationToken cancellationToken)
    {
        return _context.Tenants
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }
}
