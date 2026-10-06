using CommonTestUtilities.Data.Repositories.Tenants;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Queries.Tenants;
using DotCruz.Tenants.Application.UseCases.Tenants.Queries.GetTenantMembers;
using DotCruz.Tenants.Domain.Entities.Tenants;

namespace UseCases.Test.Tenants.GetTenantMembers;

public class GetTenantMembersQueryHandlerTests
{
    [Fact]
    public async Task Success()
    {
        var tenantId = Guid.NewGuid();
        var members = new List<TenantMember>
        {
            TenantMemberBuilder.Build(tenantId: tenantId),
            TenantMemberBuilder.Build(tenantId: tenantId)
        };

        var query = GetTenantMembersQueryBuilder.Build(tenantId: tenantId);
        var handler = CreateHandler(tenantId, members);

        var result = await handler.Handle(query, TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task Success_Empty()
    {
        var tenantId = Guid.NewGuid();
        var members = new List<TenantMember>();

        var query = GetTenantMembersQueryBuilder.Build(tenantId: tenantId);
        var handler = CreateHandler(tenantId, members);

        var result = await handler.Handle(query, TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    private static GetTenantMembersQueryHandler CreateHandler(Guid tenantId, IReadOnlyCollection<TenantMember> members)
    {
        var tenantReadRepository = new TenantReadRepositoryBuilder();
        tenantReadRepository.GetMembersAsync(tenantId, members, TestContext.Current.CancellationToken);

        return new GetTenantMembersQueryHandler(tenantReadRepository.Build());
    }
}
