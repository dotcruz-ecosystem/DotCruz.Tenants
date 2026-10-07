using CommonTestUtilities.Entities;
using CommonTestUtilities.ValueObjects.Communication;
using DotCruz.Workspaces.Domain.Entities.Workspaces;
using DotCruz.Workspaces.Domain.Enums.Workspaces;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;

namespace Domain.Test.Entities;

public class WorkspaceMemberTests
{
    [Fact]
    public void Success_Create_Member()
    {
        var WorkspaceId = Guid.NewGuid();
        var email = EmailBuilder.Build();
        var member = WorkspaceMember.Create(WorkspaceId, email, WorkspaceRole.Member);

        Assert.NotNull(member);
        Assert.Equal(WorkspaceId, member.WorkspaceId);
        Assert.Equal(email.Value, member.Email.Value);
        Assert.Equal(WorkspaceRole.Member, member.Role);
        Assert.Equal(MemberStatus.Invited, member.Status);
        Assert.Null(member.UserId);
        Assert.Null(member.JoinedAt);
    }

    [Fact]
    public void Success_Create_Active_Member_Has_JoinedAt()
    {
        var WorkspaceId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var email = EmailBuilder.Build();
        var member = WorkspaceMember.Create(WorkspaceId, email, WorkspaceRole.Owner, userId, MemberStatus.Active);

        Assert.Equal(MemberStatus.Active, member.Status);
        Assert.Equal(userId, member.UserId);
        Assert.NotNull(member.JoinedAt);
    }

    [Fact]
    public void Error_Create_Empty_WorkspaceId()
    {
        var email = EmailBuilder.Build();
        var exception = Assert.Throws<ErrorOnValidationException>(() =>
            WorkspaceMember.Create(Guid.Empty, email, WorkspaceRole.Member));

        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.TENANT_ID_REQUIRED, message);
    }

    [Fact]
    public void Success_AcceptInvite()
    {
        var member = WorkspaceMemberBuilder.Build(status: MemberStatus.Invited);
        var userId = Guid.NewGuid();

        member.AcceptInvite(userId);

        Assert.Equal(MemberStatus.Active, member.Status);
        Assert.Equal(userId, member.UserId);
        Assert.NotNull(member.JoinedAt);
    }

    [Fact]
    public void Error_AcceptInvite_Empty_UserId()
    {
        var member = WorkspaceMemberBuilder.Build(status: MemberStatus.Invited);

        var exception = Assert.Throws<ErrorOnValidationException>(() =>
            member.AcceptInvite(Guid.Empty));

        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.USER_ID_REQUIRED, message);
    }

    [Fact]
    public void Error_AcceptInvite_When_Not_Invited()
    {
        var member = WorkspaceMemberBuilder.Build(status: MemberStatus.Active);

        var exception = Assert.Throws<ErrorOnValidationException>(() =>
            member.AcceptInvite(Guid.NewGuid()));

        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.MEMBER_STATUS_INVALID, message);
    }

    [Fact]
    public void Success_UpdateRole()
    {
        var member = WorkspaceMemberBuilder.Build(role: WorkspaceRole.Member);

        member.UpdateRole(WorkspaceRole.Admin);

        Assert.Equal(WorkspaceRole.Admin, member.Role);
    }

    [Fact]
    public void Success_Suspend_And_Activate()
    {
        var member = WorkspaceMemberBuilder.Build(status: MemberStatus.Active);

        member.Suspend();
        Assert.Equal(MemberStatus.Suspended, member.Status);

        member.Activate();
        Assert.Equal(MemberStatus.Active, member.Status);
    }

    [Fact]
    public void Error_Suspend_When_Already_Suspended()
    {
        var member = WorkspaceMemberBuilder.Build(status: MemberStatus.Suspended);

        var exception = Assert.Throws<ErrorOnValidationException>(() => member.Suspend());
        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.MEMBER_ALREADY_SUSPENDED, message);
    }

    [Fact]
    public void Error_Activate_When_Already_Active()
    {
        var member = WorkspaceMemberBuilder.Build(status: MemberStatus.Active);

        var exception = Assert.Throws<ErrorOnValidationException>(() => member.Activate());
        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.MEMBER_ALREADY_ACTIVE, message);
    }
}
