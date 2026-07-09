using Bogus;
using DotCruz.Tenants.Application.UseCases.Tenants.Queries.GetTenantBySlug;

namespace CommonTestUtilities.Queries.Tenants;

public class GetTenantBySlugQueryBuilder
{
    public static GetTenantBySlugQuery Build(string? slug = null)
    {
        var faker = new Faker();
        return new GetTenantBySlugQuery(slug ?? faker.Internet.DomainWord());
    }
}
