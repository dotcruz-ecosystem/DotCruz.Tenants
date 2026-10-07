using CommonTestUtilities.Commands.Tenants;
using CommonTestUtilities.Data;
using CommonTestUtilities.Data.Repositories.Workspaces;
using CommonTestUtilities.Entities;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.SuspendTenant;
using DotCruz.Workspaces.Domain.Entities.Workspaces;
using DotCruz.Workspaces.Domain.Enums.Workspaces;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;

namespace UseCases.Test.Tenants.SuspendTenant;

public class SuspendTenantCommandHandlerTests
{
    [Fact]
    public async Task Success()
    {
        var Workspace = new WorkspaceBuilder().Build();
        var command = SuspendTenantCommandBuilder.Build(id: Workspace.Id);
        var handler = CreateHandler(Workspace);

        await handler.Handle(command, TestContext.Current.CancellationToken);

        Assert.Equal(WorkspaceStatus.Suspended, Workspace.Status);
        Assert.NotNull(Workspace.SuspensionReason);
        Assert.Equal(command.Reason, Workspace.SuspensionReason.Value);
    }

    [Fact]
    public async Task Error_Tenant_Not_Found()
    {
        var command = SuspendTenantCommandBuilder.Build();
        var handler = CreateHandler();

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.TENANT_NOT_FOUND, message);
    }

    private static SuspendTenantCommandHandler CreateHandler(Workspace? Workspace = null)
    {
        var WorkspaceWriteRepository = new WorkspaceWriteRepositoryBuilder();

        if (Workspace != null)
            WorkspaceWriteRepository.GetByIdToUpdate(Workspace, TestContext.Current.CancellationToken);

        var unitOfWork = UnitOfWorkerBuilder.Build();

        return new SuspendTenantCommandHandler(
            WorkspaceWriteRepository.Build(),
            unitOfWork
        );
    }
}
