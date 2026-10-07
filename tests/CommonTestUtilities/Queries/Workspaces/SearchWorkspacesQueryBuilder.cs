using Bogus;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.SearchTenants;
using DotCruz.Workspaces.Domain.Enums.Workspaces;

namespace CommonTestUtilities.Queries.Workspaces;

public class SearchTenantsQueryBuilder
{
    public static SearchTenantsQuery Build(
        int? pageNumber = null,
        int? pageSize = null,
        WorkspaceStatus? status = null,
        PlanType? plan = null,
        string? searchTerm = null
    )
    {
        var faker = new Faker();

        return new SearchTenantsQuery(
            PageNumber: pageNumber ?? 1,
            PageSize: pageSize ?? 10,
            Status: status,
            Plan: plan,
            SearchTerm: searchTerm
        );
    }
}
