using CommonTestUtilities.Data.Repositories.Workspaces;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Queries.Workspaces;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetUserWorkspaces;
using DotCruz.Workspaces.Domain.Entities.Workspaces;
using DotCruz.Workspaces.Domain.Enums.Workspaces;

namespace UseCases.Test.Tenants.GetUserWorkspaces;

public class GetUserWorkspacesQueryHandlerTests
{
    [Fact]
    public async Task Success()
    {
        var userId = Guid.NewGuid();
        var Workspace = new WorkspaceBuilder().Build();
        var member = WorkspaceMemberBuilder.Build(
            WorkspaceId: Workspace.Id,
            role: WorkspaceRole.Admin,
            userId: userId,
            status: MemberStatus.Active);

        var workspaces = new List<(Workspace Workspace, WorkspaceMember Member)>
        {
            (Workspace, member)
        };

        var query = GetUserWorkspacesQueryBuilder.Build(userId: userId);
        var handler = CreateHandler(userId, workspaces);

        var result = await handler.Handle(query, TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        var workspace = Assert.Single(result);
        Assert.Equal(Workspace.Id, workspace.WorkspaceId);
        Assert.Equal(Workspace.Name.Value, workspace.WorkspaceName);
        Assert.Equal(Workspace.Slug.Value, workspace.WorkspaceSlug);
        Assert.Equal(Workspace.Type.ToString(), workspace.WorkspaceType);
        Assert.Equal(Workspace.Status.ToString(), workspace.WorkspaceStatus);
        Assert.Equal(member.Role.ToString(), workspace.Role);
        Assert.Equal(member.Status.ToString(), workspace.MemberStatus);
        Assert.Equal(Workspace.Subscription.Plan.ToString(), workspace.Plan);
    }

    [Fact]
    public async Task Success_Empty()
    {
        var userId = Guid.NewGuid();
        var workspaces = new List<(Workspace Workspace, WorkspaceMember Member)>();

        var query = GetUserWorkspacesQueryBuilder.Build(userId: userId);
        var handler = CreateHandler(userId, workspaces);

        var result = await handler.Handle(query, TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    private static GetUserWorkspacesQueryHandler CreateHandler(
        Guid userId,
        IReadOnlyCollection<(Workspace Workspace, WorkspaceMember Member)> workspaces)
    {
        var WorkspaceReadRepository = new WorkspaceReadRepositoryBuilder();
        WorkspaceReadRepository.GetUserWorkspacesAsync(userId, workspaces, TestContext.Current.CancellationToken);

        return new GetUserWorkspacesQueryHandler(WorkspaceReadRepository.Build());
    }
}
