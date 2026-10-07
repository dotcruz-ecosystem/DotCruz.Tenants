using CommonTestUtilities.Commands.Tenants;
using CommonTestUtilities.Data;
using CommonTestUtilities.Data.Repositories.Workspaces;
using CommonTestUtilities.Entities;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.ActivateTenant;
using DotCruz.Workspaces.Domain.Entities.Workspaces;
using DotCruz.Workspaces.Domain.Enums.Workspaces;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;

namespace UseCases.Test.Tenants.ActivateTenant;

public class ActivateTenantCommandHandlerTests
{
    [Fact]
    public async Task Success()
    {
        var Workspace = new WorkspaceBuilder().Build();
        var command = ActivateTenantCommandBuilder.Build(id: Workspace.Id);
        var handler = CreateHandler(Workspace);

        await handler.Handle(command, TestContext.Current.CancellationToken);

        Assert.True(Workspace.Status == WorkspaceStatus.Active || Workspace.Status == WorkspaceStatus.Trialing);
        Assert.Null(Workspace.SuspensionReason);
    }

    [Fact]
    public async Task Error_Tenant_Not_Found()
    {
        var command = ActivateTenantCommandBuilder.Build();
        var handler = CreateHandler();

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.TENANT_NOT_FOUND, message);
    }

    private static ActivateTenantCommandHandler CreateHandler(Workspace? Workspace = null)
    {
        var WorkspaceWriteRepository = new WorkspaceWriteRepositoryBuilder();

        if (Workspace != null)
            WorkspaceWriteRepository.GetByIdToUpdate(Workspace, TestContext.Current.CancellationToken);

        var unitOfWork = UnitOfWorkerBuilder.Build();

        return new ActivateTenantCommandHandler(
            WorkspaceWriteRepository.Build(),
            unitOfWork
        );
    }
}
