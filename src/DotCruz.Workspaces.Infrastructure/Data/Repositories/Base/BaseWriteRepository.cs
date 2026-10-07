using DotCruz.Workspaces.Application.Abstractions.Data.Repositories.Base;
using DotCruz.Workspaces.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace DotCruz.Workspaces.Infrastructure.Data.Repositories.Base;

public class BaseWriteRepository<TEntity>(WorkspaceDbContext context) : IBaseWriteRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly WorkspaceDbContext _context = context;
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
