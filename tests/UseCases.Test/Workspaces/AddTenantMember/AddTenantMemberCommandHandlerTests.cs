using CommonTestUtilities.Commands.Tenants;
using CommonTestUtilities.Data;
using CommonTestUtilities.Data.Repositories.Workspaces;
using CommonTestUtilities.Entities;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.AddWorkspaceMember;
using DotCruz.Workspaces.Domain.Entities.Workspaces;
using DotCruz.Workspaces.Domain.Enums.Workspaces;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;

namespace UseCases.Test.Tenants.AddWorkspaceMember;

public class AddWorkspaceMemberCommandHandlerTests
{
    [Fact]
    public async Task Success()
    {
        var Workspace = new WorkspaceBuilder().Build();
        var command = AddWorkspaceMemberCommandBuilder.Build(WorkspaceId: Workspace.Id, email: "collab@dotcruz.com", role: WorkspaceRole.Member);
        var handler = CreateHandler(Workspace);

        var memberId = await handler.Handle(command, TestContext.Current.CancellationToken);

        Assert.NotEqual(Guid.Empty, memberId);
        var member = Workspace.Members.FirstOrDefault(m => m.Id == memberId);
        Assert.NotNull(member);
        Assert.Equal("collab@dotcruz.com", member.Email.Value);
        Assert.Equal(WorkspaceRole.Member, member.Role);
    }

    [Fact]
    public async Task Error_Tenant_Not_Found()
    {
        var command = AddWorkspaceMemberCommandBuilder.Build();
        var handler = CreateHandler();

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.TENANT_NOT_FOUND, message);
    }

    [Fact]
    public async Task Error_Tenant_Inactive()
    {
        var Workspace = new WorkspaceBuilder().Build();
        Workspace.Deactivate();

        var command = AddWorkspaceMemberCommandBuilder.Build(WorkspaceId: Workspace.Id);
        var handler = CreateHandler(Workspace);

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        Assert.Contains(exception.GetErrorsMessages(), m => m == ResourceMessagesException.TENANT_INACTIVE_CANNOT_ADD_MEMBER);
    }

    [Fact]
    public async Task Error_Duplicate_Email()
    {
        var Workspace = new WorkspaceBuilder().Build();
        var existingEmail = "duplicate@dotcruz.com";
        Workspace.AddMember(DotCruz.Workspaces.Domain.ValueObjects.Communication.Email.Create(existingEmail));

        var command = AddWorkspaceMemberCommandBuilder.Build(WorkspaceId: Workspace.Id, email: existingEmail);
        var handler = CreateHandler(Workspace);

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        Assert.Contains(exception.GetErrorsMessages(), m => m == ResourceMessagesException.MEMBER_ALREADY_EXISTS);
    }

    private static AddWorkspaceMemberCommandHandler CreateHandler(Workspace? Workspace = null)
    {
        var WorkspaceWriteRepository = new WorkspaceWriteRepositoryBuilder();

        if (Workspace != null)
            WorkspaceWriteRepository.GetByIdWithMembersToUpdate(Workspace, TestContext.Current.CancellationToken);

        var unitOfWork = UnitOfWorkerBuilder.Build();

        return new AddWorkspaceMemberCommandHandler(
            WorkspaceWriteRepository.Build(),
            unitOfWork
        );
    }
}
