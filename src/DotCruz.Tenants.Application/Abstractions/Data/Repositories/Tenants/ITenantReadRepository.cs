using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Base;
using DotCruz.Tenants.Domain.Entities.Tenants;
using DotCruz.Tenants.Domain.Enums.Tenants;

namespace DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;

public interface ITenantReadRepository : IBaseReadRepository<Tenant>
{
    Task<bool> ExistsWithSlugAsync(string slug, CancellationToken cancellationToken);
    Task<bool> ExistsWithDocumentAsync(string documentNumber, CancellationToken cancellationToken);
    Task<Tenant?> GetBySlugAsync(string slug, CancellationToken cancellationToken);
    Task<(IReadOnlyCollection<Tenant> Items, int TotalCount)> SearchAsync(
        int pageNumber,
        int pageSize,
        TenantStatus? status,
        PlanType? plan,
        string? searchTerm,
        CancellationToken cancellationToken);
    Task<IReadOnlyCollection<TenantMember>> GetMembersAsync(Guid tenantId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<(Tenant Tenant, TenantMember Member)>> GetUserWorkspacesAsync(Guid userId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<(Tenant Tenant, TenantMember Member)>> GetUserWorkspacesByEmailAsync(string email, CancellationToken cancellationToken);
}
