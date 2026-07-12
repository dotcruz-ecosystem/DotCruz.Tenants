using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;
using DotCruz.Tenants.Domain.Entities.Tenants;
using DotCruz.Tenants.Domain.Enums.Tenants;
using DotCruz.Tenants.Domain.ValueObjects.Tenants;
using DotCruz.Tenants.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace DotCruz.Tenants.Infrastructure.Data.Repositories.Tenants;

public class TenantReadRepository(TenantDbContext context) : BaseReadRepository<Tenant>(context), ITenantReadRepository
{
    public async Task<bool> ExistsWithSlugAsync(string slug, CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking().AnyAsync(t => t.Slug == TenantSlug.Create(slug), cancellationToken);
    }

    public async Task<bool> ExistsWithDocumentAsync(string documentNumber, CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking().AnyAsync(t => t.Document.Number == documentNumber, cancellationToken);
    }

    public async Task<Tenant?> GetBySlugAsync(string slug, CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(t => t.Slug == TenantSlug.Create(slug), cancellationToken);
    }

    public async Task<(IReadOnlyCollection<Tenant> Items, int TotalCount)> SearchAsync(
        int pageNumber,
        int pageSize,
        TenantStatus? status,
        PlanType? plan,
        string? searchTerm,
        CancellationToken cancellationToken)
    {
        var query = _dbSet.AsNoTracking();

        if (status.HasValue)
        {
            query = query.Where(t => t.Status == status.Value);
        }

        if (plan.HasValue)
        {
            query = query.Where(t => t.Subscription.Plan == plan.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(t => EF.Property<string>(t, "Name").Contains(searchTerm) || 
                                     EF.Property<string>(t, "Slug").Contains(searchTerm));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(t => EF.Property<string>(t, "Name"))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
