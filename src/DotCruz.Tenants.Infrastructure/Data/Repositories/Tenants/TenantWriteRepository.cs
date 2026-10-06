using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;
using DotCruz.Tenants.Domain.Entities.Tenants;
using DotCruz.Tenants.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace DotCruz.Tenants.Infrastructure.Data.Repositories.Tenants;

public class TenantWriteRepository(TenantDbContext context) 
    : BaseWriteRepository<Tenant>(context), ITenantWriteRepository
{
    public Task<Tenant?> GetByIdWithMembersToUpdateAsync(Guid id, CancellationToken cancellationToken)
    {
        return _context.Tenants
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }
}
