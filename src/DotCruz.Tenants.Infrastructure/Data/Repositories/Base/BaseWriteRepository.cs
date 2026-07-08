using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Base;
using DotCruz.Tenants.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace DotCruz.Tenants.Infrastructure.Data.Repositories.Base;

public class BaseWriteRepository<TEntity>(TenantDbContext context) : IBaseWriteRepository<TEntity> where TEntity : BaseEntity
{
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public Task<TEntity?> GetByIdToUpdateAsync(Guid id, CancellationToken cancellationToken)
    {
        return _dbSet.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
}
