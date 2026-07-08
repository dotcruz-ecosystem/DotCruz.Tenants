using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;
using DotCruz.Tenants.Domain.Entities.Tenants;
using DotCruz.Tenants.Infrastructure.Data.Repositories.Base;

namespace DotCruz.Tenants.Infrastructure.Data.Repositories.Tenants;

public class TenantWriteRepository(TenantDbContext context) 
    : BaseWriteRepository<Tenant>(context), ITenantWriteRepository;
