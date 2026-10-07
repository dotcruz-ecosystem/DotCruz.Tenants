using DotCruz.Workspaces.Application.Abstractions.Data.Repositories.Workspaces;
using DotCruz.Workspaces.Application.Mappers.Workspaces;
using MediatR;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.SearchTenants;

public class SearchTenantsQueryHandler : IRequestHandler<SearchTenantsQuery, SearchTenantsResponse>
{
    private readonly IWorkspaceReadRepository _WorkspaceReadRepository;

    public SearchTenantsQueryHandler(IWorkspaceReadRepository WorkspaceReadRepository)
    {
        _WorkspaceReadRepository = WorkspaceReadRepository;
    }

    public async Task<SearchTenantsResponse> Handle(SearchTenantsQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _WorkspaceReadRepository.SearchAsync(
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
