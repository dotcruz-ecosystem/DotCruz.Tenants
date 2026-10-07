using DotCruz.Workspaces.Application.Abstractions.Data;

namespace DotCruz.Workspaces.Infrastructure.Data;

public class UnitOfWork(WorkspaceDbContext context) : IUnitOfWork
{
    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}
