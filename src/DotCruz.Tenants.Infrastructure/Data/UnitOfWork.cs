using DotCruz.Tenants.Application.Abstractions.Data;

namespace DotCruz.Tenants.Infrastructure.Data;

public class UnitOfWork(TenantDbContext context) : IUnitOfWork
{
    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}
