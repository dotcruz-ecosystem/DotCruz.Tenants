using CommonTestUtilities.Data.Repositories.Workspaces;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Queries.Workspaces;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetWorkspaceMembers;
using DotCruz.Workspaces.Domain.Entities.Workspaces;

namespace UseCases.Test.Tenants.GetWorkspaceMembers;

public class GetWorkspaceMembersQueryHandlerTests
{
    [Fact]
    public async Task Success()
    {
        var WorkspaceId = Guid.NewGuid();
        var members = new List<WorkspaceMember>
        {
            WorkspaceMemberBuilder.Build(WorkspaceId: WorkspaceId),
            WorkspaceMemberBuilder.Build(WorkspaceId: WorkspaceId)
        };

        var query = GetWorkspaceMembersQueryBuilder.Build(WorkspaceId: WorkspaceId);
        var handler = CreateHandler(WorkspaceId, members);

        var result = await handler.Handle(query, TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task Success_Empty()
    {
        var WorkspaceId = Guid.NewGuid();
        var members = new List<WorkspaceMember>();

        var query = GetWorkspaceMembersQueryBuilder.Build(WorkspaceId: WorkspaceId);
        var handler = CreateHandler(WorkspaceId, members);

        var result = await handler.Handle(query, TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    private static GetWorkspaceMembersQueryHandler CreateHandler(Guid WorkspaceId, IReadOnlyCollection<WorkspaceMember> members)
    {
        var WorkspaceReadRepository = new WorkspaceReadRepositoryBuilder();
        WorkspaceReadRepository.GetMembersAsync(WorkspaceId, members, TestContext.Current.CancellationToken);

        return new GetWorkspaceMembersQueryHandler(WorkspaceReadRepository.Build());
    }
}
