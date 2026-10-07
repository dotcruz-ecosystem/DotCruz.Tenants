using CommonTestUtilities.Commands.Tenants;
using CommonTestUtilities.Data;
using CommonTestUtilities.Data.Repositories.Workspaces;
using CommonTestUtilities.Entities;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.UpdateWorkspaceContact;
using DotCruz.Workspaces.Domain.Entities.Workspaces;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;

namespace UseCases.Test.Tenants.UpdateWorkspaceContact;

public class UpdateWorkspaceContactCommandHandlerTests
{
    [Fact]
    public async Task Success()
    {
        var Workspace = new WorkspaceBuilder().Build();
        var command = UpdateWorkspaceContactCommandBuilder.Build(id: Workspace.Id);
        var handler = CreateHandler(Workspace);

        await handler.Handle(command, TestContext.Current.CancellationToken);

        Assert.Equal(command.WorkspaceContact.Email.ToLowerInvariant(), Workspace.Contact.Email.Value);
        Assert.Equal(command.WorkspaceContact.PhoneCountryCode, Workspace.Contact.Phone.CountryCode);
        Assert.Equal(command.WorkspaceContact.PhoneNationalNumber, Workspace.Contact.Phone.NationalNumber);
    }

    [Fact]
    public async Task Error_Tenant_Not_Found()
    {
        var command = UpdateWorkspaceContactCommandBuilder.Build();
        var handler = CreateHandler();

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.TENANT_NOT_FOUND, message);
    }

    private static UpdateWorkspaceContactCommandHandler CreateHandler(Workspace? Workspace = null)
    {
        var WorkspaceWriteRepository = new WorkspaceWriteRepositoryBuilder();

        if (Workspace != null)
            WorkspaceWriteRepository.GetByIdToUpdate(Workspace, TestContext.Current.CancellationToken);

        var unitOfWork = UnitOfWorkerBuilder.Build();

        return new UpdateWorkspaceContactCommandHandler(
            WorkspaceWriteRepository.Build(),
            unitOfWork
        );
    }
}
