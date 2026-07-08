using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;
using DotCruz.Tenants.Application.Mappers.Tenants;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.SearchTenants;

public class SearchTenantsQueryHandler : IRequestHandler<SearchTenantsQuery, SearchTenantsResponse>
{
    private readonly ITenantReadRepository _tenantReadRepository;

    public SearchTenantsQueryHandler(ITenantReadRepository tenantReadRepository)
    {
        _tenantReadRepository = tenantReadRepository;
    }

    public async Task<SearchTenantsResponse> Handle(SearchTenantsQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _tenantReadRepository.SearchAsync(
            request.PageNumber,
            request.PageSize,
            request.Status,
            request.Plan,
            request.SearchTerm,
            cancellationToken
        );

        var dtos = items.Select(t => t.ToDto()).ToList();

        return new SearchTenantsResponse(
            Items: dtos,
            PageNumber: request.PageNumber,
            PageSize: request.PageSize,
            TotalCount: totalCount
        );
    }
}
