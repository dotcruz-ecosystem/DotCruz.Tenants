using CommonTestUtilities.Entities;
using CommonTestUtilities.Queries.Workspaces;
using CommonTestUtilities.Data.Repositories.Workspaces;
using DotCruz.Workspaces.Domain.Entities.Workspaces;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetTenantBySlug;

namespace UseCases.Test.Tenants.GetTenantBySlug;

public class GetTenantBySlugQueryHandlerTests
{
    [Fact]
    public async Task Success()
    {
        var Workspace = new WorkspaceBuilder().Build();
        var query = GetTenantBySlugQueryBuilder.Build(slug: Workspace.Slug.Value);
        var handler = CreateHandler(Workspace);

        var result = await handler.Handle(query, TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.Equal(Workspace.Id, result.Id);
        Assert.Equal(Workspace.Name.Value, result.Name);
        Assert.Equal(Workspace.Slug.Value, result.Slug);
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

    private static GetTenantBySlugQueryHandler CreateHandler(Workspace? Workspace = null)
    {
        var WorkspaceReadRepository = new WorkspaceReadRepositoryBuilder();

        if (Workspace != null)
            WorkspaceReadRepository.GetBySlugAsync(Workspace.Slug.Value, Workspace, TestContext.Current.CancellationToken);

        return new GetTenantBySlugQueryHandler(WorkspaceReadRepository.Build());
    }
}
