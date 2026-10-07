using CommonTestUtilities.Entities;
using CommonTestUtilities.Queries.Workspaces;
using CommonTestUtilities.Data.Repositories.Workspaces;
using DotCruz.Workspaces.Domain.Entities.Workspaces;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.SearchTenants;

namespace UseCases.Test.Tenants.SearchTenants;

public class SearchTenantsQueryHandlerTests
{
    [Fact]
    public async Task Success()
    {
        var Workspace = new WorkspaceBuilder().Build();
        var query = SearchTenantsQueryBuilder.Build();
        var handler = CreateHandler(new List<Workspace> { Workspace }, 1);

        var result = await handler.Handle(query, TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        var item = Assert.Single(result.Items);
        Assert.Equal(Workspace.Id, item.Id);
        Assert.Equal(1, result.TotalCount);
        Assert.Equal(query.PageNumber, result.PageNumber);
        Assert.Equal(query.PageSize, result.PageSize);
    }

    private static SearchTenantsQueryHandler CreateHandler(IReadOnlyCollection<Workspace> items, int totalCount)
    {
        var WorkspaceReadRepository = new WorkspaceReadRepositoryBuilder();

        WorkspaceReadRepository.SearchAsync(
            1,
            10,
            null,
            null,
            null,
            items,
            totalCount,
            TestContext.Current.CancellationToken
        );

        return new SearchTenantsQueryHandler(WorkspaceReadRepository.Build());
    }
}
