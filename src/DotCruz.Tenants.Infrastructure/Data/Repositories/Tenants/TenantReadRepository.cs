using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;
using DotCruz.Tenants.Domain.Entities.Tenants;
using DotCruz.Tenants.Infrastructure.Data.Repositories.Base;

namespace DotCruz.Tenants.Infrastructure.Data.Repositories.Tenants;

public class TenantReadRepository(TenantDbContext context)
    : BaseReadRepository<Tenant>(context), ITenantReadRepository;
