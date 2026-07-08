using Bogus;
using DotCruz.Tenants.Application.UseCases.Tenants.SearchTenants;
using DotCruz.Tenants.Domain.Enums.Tenants;

namespace CommonTestUtilities.Queries.Tenants;

public class SearchTenantsQueryBuilder
{
    public static SearchTenantsQuery Build(
        int? pageNumber = null,
        int? pageSize = null,
        TenantStatus? status = null,
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
