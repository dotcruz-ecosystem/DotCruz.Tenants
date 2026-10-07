using Bogus;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetTenantBySlug;

namespace CommonTestUtilities.Queries.Workspaces;

public class GetTenantBySlugQueryBuilder
{
    public static GetTenantBySlugQuery Build(string? slug = null)
    {
        var faker = new Faker();
        return new GetTenantBySlugQuery(slug ?? faker.Internet.DomainWord());
    }
}
