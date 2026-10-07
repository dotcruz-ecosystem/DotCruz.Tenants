using CommonTestUtilities.Commands.Tenants;
using CommonTestUtilities.Data;
using CommonTestUtilities.Data.Repositories.Workspaces;
using CommonTestUtilities.Entities;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.RemoveWorkspaceMember;
using DotCruz.Workspaces.Domain.Entities.Workspaces;
using DotCruz.Workspaces.Domain.Enums.Workspaces;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;
using DotCruz.Workspaces.Domain.ValueObjects.Communication;

namespace UseCases.Test.Tenants.RemoveWorkspaceMember;

public class RemoveWorkspaceMemberCommandHandlerTests
{
    [Fact]
    public async Task Success()
    {
        var Workspace = new WorkspaceBuilder().Build();
        var member = Workspace.AddMember(Email.Create("member@dotcruz.com"), WorkspaceRole.Member);

        var command = RemoveWorkspaceMemberCommandBuilder.Build(WorkspaceId: Workspace.Id, memberId: member.Id);
        var handler = CreateHandler(Workspace);

        await handler.Handle(command, TestContext.Current.CancellationToken);

        Assert.NotNull(member.DeletedAt);
    }

    [Fact]
    public async Task Error_Tenant_Not_Found()
    {
        var command = RemoveWorkspaceMemberCommandBuilder.Build();
        var handler = CreateHandler();

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.TENANT_NOT_FOUND, message);
    }

    [Fact]
    public async Task Error_Member_Not_Found()
    {
        var Workspace = new WorkspaceBuilder().Build();
        var command = RemoveWorkspaceMemberCommandBuilder.Build(WorkspaceId: Workspace.Id, memberId: Guid.NewGuid());
        var handler = CreateHandler(Workspace);

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.MEMBER_NOT_FOUND, message);
    }

    [Fact]
    public async Task Error_Cannot_Remove_Last_Owner()
    {
        var Workspace = new WorkspaceBuilder().WithOwnerMember().Build();
        var owner = Workspace.Members.First(m => m.Role == WorkspaceRole.Owner);

        var command = RemoveWorkspaceMemberCommandBuilder.Build(WorkspaceId: Workspace.Id, memberId: owner.Id);
        var handler = CreateHandler(Workspace);

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        Assert.Contains(exception.GetErrorsMessages(), m => m == ResourceMessagesException.CANNOT_REMOVE_LAST_OWNER);
    }

    private static RemoveWorkspaceMemberCommandHandler CreateHandler(Workspace? Workspace = null)
    {
        var WorkspaceWriteRepository = new WorkspaceWriteRepositoryBuilder();

        if (Workspace != null)
            WorkspaceWriteRepository.GetByIdWithMembersToUpdate(Workspace, TestContext.Current.CancellationToken);

        var unitOfWork = UnitOfWorkerBuilder.Build();

        return new RemoveWorkspaceMemberCommandHandler(
            WorkspaceWriteRepository.Build(),
            unitOfWork
        );
    }
}
