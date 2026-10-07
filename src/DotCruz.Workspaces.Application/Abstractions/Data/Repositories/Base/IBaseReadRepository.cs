using DotCruz.Workspaces.Domain.Entities.Base;

namespace DotCruz.Workspaces.Application.Abstractions.Data.Repositories.Base;

public interface IBaseReadRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
}