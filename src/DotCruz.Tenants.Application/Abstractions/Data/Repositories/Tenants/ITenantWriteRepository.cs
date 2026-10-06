using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Base;
using DotCruz.Tenants.Domain.Entities.Tenants;

namespace DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;

public interface ITenantWriteRepository : IBaseWriteRepository<Tenant>
{
    Task<Tenant?> GetByIdWithMembersToUpdateAsync(Guid id, CancellationToken cancellationToken);
}
