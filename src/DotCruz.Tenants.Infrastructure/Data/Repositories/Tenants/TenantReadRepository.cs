using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;
using DotCruz.Tenants.Domain.Entities.Tenants;
using DotCruz.Tenants.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace DotCruz.Tenants.Infrastructure.Data.Repositories.Tenants;

public class TenantReadRepository(TenantDbContext context) : BaseReadRepository<Tenant>(context), ITenantReadRepository
{
    public async Task<bool> ExistsWithSlugAsync(string slug, CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking().AnyAsync(t => t.Slug.Value == slug, cancellationToken);
    }

    public async Task<bool> ExistsWithDocumentAsync(string documentNumber, CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking().AnyAsync(t => t.Document.Number == documentNumber, cancellationToken);
    }
}
