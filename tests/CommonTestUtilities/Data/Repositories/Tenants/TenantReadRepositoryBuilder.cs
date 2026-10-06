using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;
using DotCruz.Tenants.Domain.Entities.Tenants;
using DotCruz.Tenants.Domain.Enums.Tenants;
using Moq;

namespace CommonTestUtilities.Data.Repositories.Tenants;

public class TenantReadRepositoryBuilder
{
    private readonly Mock<ITenantReadRepository> _tenantReadRepositoryMock = new();
    
    public void ExistsWithSlugAsync(string slug, CancellationToken cancellationToken)
    {
        _tenantReadRepositoryMock
            .Setup(r => r.ExistsWithSlugAsync(slug, cancellationToken))
            .ReturnsAsync(true);
    }

    public void ExistsWithDocumentAsync(string documentNumber, CancellationToken cancellationToken)
    {
        _tenantReadRepositoryMock
            .Setup(r => r.ExistsWithDocumentAsync(documentNumber, cancellationToken))
            .ReturnsAsync(true);
    }

    public void GetByIdAsync(Guid id, Tenant? tenant, CancellationToken cancellationToken)
    {
        _tenantReadRepositoryMock
            .Setup(r => r.GetByIdAsync(id, cancellationToken))
            .ReturnsAsync(tenant);
    }

    public void GetBySlugAsync(string slug, Tenant? tenant, CancellationToken cancellationToken)
    {
        _tenantReadRepositoryMock
            .Setup(r => r.GetBySlugAsync(slug, cancellationToken))
            .ReturnsAsync(tenant);
    }

    public void SearchAsync(
        int pageNumber,
        int pageSize,
        TenantStatus? status,
        PlanType? plan,
        string? searchTerm,
        IReadOnlyCollection<Tenant> items,
        int totalCount,
        CancellationToken cancellationToken)
    {
        _tenantReadRepositoryMock
            .Setup(r => r.SearchAsync(pageNumber, pageSize, status, plan, searchTerm, cancellationToken))
            .ReturnsAsync((items, totalCount));
    }

    public void GetMembersAsync(Guid tenantId, IReadOnlyCollection<TenantMember> members, CancellationToken cancellationToken)
    {
        _tenantReadRepositoryMock
            .Setup(r => r.GetMembersAsync(tenantId, cancellationToken))
            .ReturnsAsync(members);
    }

    public void GetUserWorkspacesAsync(Guid userId, IReadOnlyCollection<(Tenant Tenant, TenantMember Member)> workspaces, CancellationToken cancellationToken)
    {
        _tenantReadRepositoryMock
            .Setup(r => r.GetUserWorkspacesAsync(userId, cancellationToken))
            .ReturnsAsync(workspaces);
    }

    public void GetUserWorkspacesByEmailAsync(string email, IReadOnlyCollection<(Tenant Tenant, TenantMember Member)> workspaces, CancellationToken cancellationToken)
    {
        _tenantReadRepositoryMock
            .Setup(r => r.GetUserWorkspacesByEmailAsync(email, cancellationToken))
            .ReturnsAsync(workspaces);
    }

    public ITenantReadRepository Build() => _tenantReadRepositoryMock.Object;
}
