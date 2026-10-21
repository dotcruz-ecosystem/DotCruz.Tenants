using CommonTestUtilities.Commands.Tenants;
using CommonTestUtilities.Data;
using CommonTestUtilities.Data.Repositories.Workspaces;
using CommonTestUtilities.Security;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.CreateWorkspace;
using DotCruz.Workspaces.Domain.Enums.Workspaces;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;

namespace UseCases.Test.Tenants.CreateWorkspace;

public class CreateWorkspaceCommandHandlerTests
{
    [Fact]
    public async Task Success_With_OwnerEmail()
    {
        var userId = Guid.NewGuid();
        var command = CreateWorkspaceCommandBuilder.Build(ownerEmail: "owner@dotcruz.com");
        var handler = CreateHandler(userId: userId);

        var result = await handler.Handle(command, TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.WorkspaceId);
        Assert.Equal(command.Name, result.WorkspaceName);
        Assert.Equal(command.Slug, result.WorkspaceSlug);
        Assert.Equal(command.Type.ToString(), result.WorkspaceType);
        Assert.Equal(WorkspaceStatus.Active.ToString(), result.WorkspaceStatus);
        Assert.Equal(WorkspaceRole.Owner.ToString(), result.Role);
        Assert.Equal(MemberStatus.Active.ToString(), result.MemberStatus);
        Assert.Equal(command.Type == WorkspaceType.Personal ? PlanType.Free.ToString() : PlanType.Trial.ToString(), result.Plan);
    }

    [Fact]
    public async Task Success_Without_OwnerEmail()
    {
        var userId = Guid.NewGuid();
        var command = CreateWorkspaceCommandBuilder.Build(ownerEmail: null);
        var handler = CreateHandler(userId: userId);

        var result = await handler.Handle(command, TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.WorkspaceId);
        Assert.Equal(command.Name, result.WorkspaceName);
        Assert.Equal(command.Slug, result.WorkspaceSlug);
        Assert.Equal(WorkspaceRole.Owner.ToString(), result.Role);
    }

    [Fact]
    public async Task Error_Unauthenticated_User()
    {
        var command = CreateWorkspaceCommandBuilder.Build();
        var handler = CreateHandler(userId: null);

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<ForbiddenException>(act);
        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE, message);
    }

    [Fact]
    public async Task Error_Slug_Already_Exists()
    {
        var userId = Guid.NewGuid();
        var command = CreateWorkspaceCommandBuilder.Build();
        var handler = CreateHandler(userId: userId, existingSlug: command.Slug);

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.SLUG_ALREADY_EXISTS, message);
    }

    private static CreateWorkspaceCommandHandler CreateHandler(Guid? userId = null, string? existingSlug = null)
    {
        var writeRepo = new WorkspaceWriteRepositoryBuilder().Build();
        var readRepoBuilder = new WorkspaceReadRepositoryBuilder();
        var unitOfWork = UnitOfWorkerBuilder.Build();
        var securityContextBuilder = new SecurityContextBuilder();

        if (userId.HasValue)
            securityContextBuilder.AsUser(userId.Value);

        if (!string.IsNullOrEmpty(existingSlug))
            readRepoBuilder.ExistsWithSlugAsync(existingSlug, TestContext.Current.CancellationToken);

        return new CreateWorkspaceCommandHandler(
            writeRepo,
            readRepoBuilder.Build(),
            unitOfWork,
            securityContextBuilder.Build()
        );
    }
}
