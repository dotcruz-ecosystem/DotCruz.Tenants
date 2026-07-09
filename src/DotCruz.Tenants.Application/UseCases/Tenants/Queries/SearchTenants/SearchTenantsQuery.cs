using DotCruz.Tenants.Domain.Enums.Tenants;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Queries.SearchTenants;

public record SearchTenantsQuery(
    int PageNumber = 1,
    int PageSize = 10,
    TenantStatus? Status = null,
    PlanType? Plan = null,
    string? SearchTerm = null
) : IRequest<SearchTenantsResponse>;
