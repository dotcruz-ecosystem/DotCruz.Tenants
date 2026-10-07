using CommonTestUtilities.Commands.Tenants;
using CommonTestUtilities.Data;
using CommonTestUtilities.Data.Repositories.Workspaces;
using CommonTestUtilities.Entities;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.UpdateWorkspaceAddress;
using DotCruz.Workspaces.Domain.Entities.Workspaces;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;

namespace UseCases.Test.Tenants.UpdateWorkspaceAddress;

public class UpdateWorkspaceAddressCommandHandlerTests
{
    [Fact]
    public async Task Success()
    {
        var Workspace = new WorkspaceBuilder().Build();
        var command = UpdateWorkspaceAddressCommandBuilder.Build(id: Workspace.Id);
        var handler = CreateHandler(Workspace);

        await handler.Handle(command, TestContext.Current.CancellationToken);

        Assert.Equal(command.WorkspaceAddress.Street, Workspace.Address.Street);
        Assert.Equal(command.WorkspaceAddress.Number, Workspace.Address.Number);
        Assert.Equal(command.WorkspaceAddress.Complement, Workspace.Address.Complement);
        Assert.Equal(command.WorkspaceAddress.Neighborhood, Workspace.Address.Neighborhood);
        Assert.Equal(command.WorkspaceAddress.City, Workspace.Address.City);
        Assert.Equal(command.WorkspaceAddress.State, Workspace.Address.State.Value);
        Assert.Equal(command.WorkspaceAddress.ZipCode.Replace("-", ""), Workspace.Address.ZipCode.Value);
    }

    [Fact]
    public async Task Error_Tenant_Not_Found()
    {
        var command = UpdateWorkspaceAddressCommandBuilder.Build();
        var handler = CreateHandler();

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.TENANT_NOT_FOUND, message);
    }

    private static UpdateWorkspaceAddressCommandHandler CreateHandler(Workspace? Workspace = null)
    {
        var WorkspaceWriteRepository = new WorkspaceWriteRepositoryBuilder();

        if (Workspace != null)
            WorkspaceWriteRepository.GetByIdToUpdate(Workspace, TestContext.Current.CancellationToken);

        var unitOfWork = UnitOfWorkerBuilder.Build();

        return new UpdateWorkspaceAddressCommandHandler(
            WorkspaceWriteRepository.Build(),
            unitOfWork
        );
    }
}
