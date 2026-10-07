using DotCruz.Workspaces.Domain.Enums.Workspaces;
using MediatR;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.SearchTenants;

public record SearchTenantsQuery(
    int PageNumber = 1,
    int PageSize = 10,
    WorkspaceStatus? Status = null,
    PlanType? Plan = null,
    string? SearchTerm = null
) : IRequest<SearchTenantsResponse>;
