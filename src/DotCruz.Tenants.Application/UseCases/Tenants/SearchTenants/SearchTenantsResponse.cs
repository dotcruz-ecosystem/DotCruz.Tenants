using DotCruz.Tenants.Application.DTOs.Tenants;

namespace DotCruz.Tenants.Application.UseCases.Tenants.SearchTenants;

public record SearchTenantsResponse(
    IReadOnlyCollection<TenantDto> Items,
    int PageNumber,
    int PageSize,
    int TotalCount
);
