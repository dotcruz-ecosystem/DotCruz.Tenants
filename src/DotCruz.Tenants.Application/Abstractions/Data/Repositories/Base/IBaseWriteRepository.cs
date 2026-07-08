using DotCruz.Tenants.Domain.Entities.Base;

namespace DotCruz.Tenants.Application.Abstractions.Data.Repositories.Base;

public interface IBaseWriteRepository<TEntity> where TEntity : BaseEntity
{
    Task AddAsync(TEntity entity);
    Task<TEntity?> GetByIdToUpdateAsync(Guid id, CancellationToken cancellationToken);
}
