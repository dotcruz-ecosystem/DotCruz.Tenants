using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Base;
using DotCruz.Tenants.Domain.Entities.Tenants;

namespace DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;

public interface ITenantReadRepository : IBaseReadRepository<Tenant>;
