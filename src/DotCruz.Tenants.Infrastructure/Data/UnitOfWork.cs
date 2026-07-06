namespace DotCruz.Tenants.Infrastructure.Data;

public class UnitOfWork(TenantDbContext context)
{
    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}
