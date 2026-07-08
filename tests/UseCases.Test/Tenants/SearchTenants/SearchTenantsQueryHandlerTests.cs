using CommonTestUtilities.Entities;
using CommonTestUtilities.Queries.Tenants;
using CommonTestUtilities.Data.Repositories.Tenants;
using DotCruz.Tenants.Application.UseCases.Tenants.SearchTenants;
using DotCruz.Tenants.Domain.Entities.Tenants;

namespace UseCases.Test.Tenants.SearchTenants;

public class SearchTenantsQueryHandlerTests
{
    [Fact]
    public async Task Success()
    {
        var tenant = new TenantBuilder().Build();
        var query = SearchTenantsQueryBuilder.Build();
        var handler = CreateHandler(new List<Tenant> { tenant }, 1);

        var result = await handler.Handle(query, TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        var item = Assert.Single(result.Items);
        Assert.Equal(tenant.Id, item.Id);
        Assert.Equal(1, result.TotalCount);
        Assert.Equal(query.PageNumber, result.PageNumber);
        Assert.Equal(query.PageSize, result.PageSize);
    }

    private static SearchTenantsQueryHandler CreateHandler(IReadOnlyCollection<Tenant> items, int totalCount)
    {
        var tenantReadRepository = new TenantReadRepositoryBuilder();

        tenantReadRepository.SearchAsync(
            1,
            10,
            null,
            null,
            null,
            items,
            totalCount,
            TestContext.Current.CancellationToken
        );

        return new SearchTenantsQueryHandler(tenantReadRepository.Build());
    }
}
