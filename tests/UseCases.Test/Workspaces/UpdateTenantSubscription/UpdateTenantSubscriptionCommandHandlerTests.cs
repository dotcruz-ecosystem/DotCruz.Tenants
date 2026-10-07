using CommonTestUtilities.Commands.Tenants;
using CommonTestUtilities.Data;
using CommonTestUtilities.Data.Repositories.Workspaces;
using CommonTestUtilities.Entities;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.UpdateWorkspaceSubscription;
using DotCruz.Workspaces.Domain.Entities.Workspaces;
using DotCruz.Workspaces.Domain.Enums.Workspaces;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;

namespace UseCases.Test.Tenants.UpdateWorkspaceSubscription;

public class UpdateWorkspaceSubscriptionCommandHandlerTests
{
    [Fact]
    public async Task Success()
    {
        var Workspace = new WorkspaceBuilder().Build();
        var command = UpdateWorkspaceSubscriptionCommandBuilder.Build(id: Workspace.Id, plan: PlanType.Pro);
        var handler = CreateHandler(Workspace);

        await handler.Handle(command, TestContext.Current.CancellationToken);

        Assert.Equal(command.Plan, Workspace.Subscription.Plan);
        Assert.Equal(50, Workspace.Subscription.Limits.MaxUsers);
        Assert.Equal(50000, Workspace.Subscription.Limits.MaxEmailsPerMonth);
    }

    [Fact]
    public async Task Error_Tenant_Not_Found()
    {
        var command = UpdateWorkspaceSubscriptionCommandBuilder.Build();
        var handler = CreateHandler();

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.TENANT_NOT_FOUND, message);
    }

    private static UpdateWorkspaceSubscriptionCommandHandler CreateHandler(Workspace? Workspace = null)
    {
        var WorkspaceWriteRepository = new WorkspaceWriteRepositoryBuilder();

        if (Workspace != null)
            WorkspaceWriteRepository.GetByIdToUpdate(Workspace, TestContext.Current.CancellationToken);

        var unitOfWork = UnitOfWorkerBuilder.Build();

        return new UpdateWorkspaceSubscriptionCommandHandler(
            WorkspaceWriteRepository.Build(),
            unitOfWork
        );
    }
}
