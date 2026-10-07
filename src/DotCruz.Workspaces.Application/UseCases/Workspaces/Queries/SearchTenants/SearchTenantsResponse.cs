using DotCruz.Workspaces.Application.DTOs.Workspaces;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.SearchTenants;

public record SearchTenantsResponse(
    IReadOnlyCollection<WorkspaceDto> Items,
    int PageNumber,
    int PageSize,
    int TotalCount
);
