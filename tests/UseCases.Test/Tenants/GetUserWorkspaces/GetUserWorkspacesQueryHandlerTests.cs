using CommonTestUtilities.Data.Repositories.Tenants;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Queries.Tenants;
using DotCruz.Tenants.Application.UseCases.Tenants.Queries.GetUserWorkspaces;
using DotCruz.Tenants.Domain.Entities.Tenants;
using DotCruz.Tenants.Domain.Enums.Tenants;

namespace UseCases.Test.Tenants.GetUserWorkspaces;

public class GetUserWorkspacesQueryHandlerTests
{
    [Fact]
    public async Task Success()
    {
        var userId = Guid.NewGuid();
        var tenant = new TenantBuilder().Build();
        var member = TenantMemberBuilder.Build(
            tenantId: tenant.Id,
            role: TenantRole.Admin,
            userId: userId,
            status: MemberStatus.Active);

        var workspaces = new List<(Tenant Tenant, TenantMember Member)>
        {
            (tenant, member)
        };

        var query = GetUserWorkspacesQueryBuilder.Build(userId: userId);
        var handler = CreateHandler(userId, workspaces);

        var result = await handler.Handle(query, TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        var workspace = Assert.Single(result);
        Assert.Equal(tenant.Id, workspace.TenantId);
        Assert.Equal(tenant.Name.Value, workspace.TenantName);
        Assert.Equal(tenant.Slug.Value, workspace.TenantSlug);
        Assert.Equal(tenant.Type.ToString(), workspace.TenantType);
        Assert.Equal(tenant.Status.ToString(), workspace.TenantStatus);
        Assert.Equal(member.Role.ToString(), workspace.Role);
        Assert.Equal(member.Status.ToString(), workspace.MemberStatus);
        Assert.Equal(tenant.Subscription.Plan.ToString(), workspace.Plan);
    }

    [Fact]
    public async Task Success_Empty()
    {
        var userId = Guid.NewGuid();
        var workspaces = new List<(Tenant Tenant, TenantMember Member)>();

        var query = GetUserWorkspacesQueryBuilder.Build(userId: userId);
        var handler = CreateHandler(userId, workspaces);

        var result = await handler.Handle(query, TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    private static GetUserWorkspacesQueryHandler CreateHandler(
        Guid userId,
        IReadOnlyCollection<(Tenant Tenant, TenantMember Member)> workspaces)
    {
        var tenantReadRepository = new TenantReadRepositoryBuilder();
        tenantReadRepository.GetUserWorkspacesAsync(userId, workspaces, TestContext.Current.CancellationToken);

        return new GetUserWorkspacesQueryHandler(tenantReadRepository.Build());
    }
}
