using CommonTestUtilities.Entities;
using CommonTestUtilities.Queries.Tenants;
using CommonTestUtilities.Data.Repositories.Tenants;
using DotCruz.Tenants.Domain.Entities.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using DotCruz.Tenants.Application.UseCases.Tenants.Queries.GetTenantBySlug;

namespace UseCases.Test.Tenants.GetTenantBySlug;

public class GetTenantBySlugQueryHandlerTests
{
    [Fact]
    public async Task Success()
    {
        var tenant = new TenantBuilder().Build();
        var query = GetTenantBySlugQueryBuilder.Build(slug: tenant.Slug.Value);
        var handler = CreateHandler(tenant);

        var result = await handler.Handle(query, TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.Equal(tenant.Id, result.Id);
        Assert.Equal(tenant.Name.Value, result.Name);
        Assert.Equal(tenant.Slug.Value, result.Slug);
    }

    [Fact]
    public async Task Error_Tenant_Not_Found()
    {
        var query = GetTenantBySlugQueryBuilder.Build();
        var handler = CreateHandler();

        Task act() => handler.Handle(query, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.TENANT_NOT_FOUND, message);
    }

    private static GetTenantBySlugQueryHandler CreateHandler(Tenant? tenant = null)
    {
        var tenantReadRepository = new TenantReadRepositoryBuilder();

        if (tenant != null)
            tenantReadRepository.GetBySlugAsync(tenant.Slug.Value, tenant, TestContext.Current.CancellationToken);

        return new GetTenantBySlugQueryHandler(tenantReadRepository.Build());
    }
}
