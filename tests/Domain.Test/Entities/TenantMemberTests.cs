using CommonTestUtilities.Entities;
using CommonTestUtilities.ValueObjects.Communication;
using DotCruz.Tenants.Domain.Entities.Tenants;
using DotCruz.Tenants.Domain.Enums.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;

namespace Domain.Test.Entities;

public class TenantMemberTests
{
    [Fact]
    public void Success_Create_Member()
    {
        var tenantId = Guid.NewGuid();
        var email = EmailBuilder.Build();
        var member = TenantMember.Create(tenantId, email, TenantRole.Member);

        Assert.NotNull(member);
        Assert.Equal(tenantId, member.TenantId);
        Assert.Equal(email.Value, member.Email.Value);
        Assert.Equal(TenantRole.Member, member.Role);
        Assert.Equal(MemberStatus.Invited, member.Status);
        Assert.Null(member.UserId);
        Assert.Null(member.JoinedAt);
    }

    [Fact]
    public void Success_Create_Active_Member_Has_JoinedAt()
    {
        var tenantId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var email = EmailBuilder.Build();
        var member = TenantMember.Create(tenantId, email, TenantRole.Owner, userId, MemberStatus.Active);

        Assert.Equal(MemberStatus.Active, member.Status);
        Assert.Equal(userId, member.UserId);
        Assert.NotNull(member.JoinedAt);
    }

    [Fact]
    public void Error_Create_Empty_TenantId()
    {
        var email = EmailBuilder.Build();
        var exception = Assert.Throws<ErrorOnValidationException>(() =>
            TenantMember.Create(Guid.Empty, email, TenantRole.Member));

        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.TENANT_ID_REQUIRED, message);
    }

    [Fact]
    public void Success_AcceptInvite()
    {
        var member = TenantMemberBuilder.Build(status: MemberStatus.Invited);
        var userId = Guid.NewGuid();

        member.AcceptInvite(userId);

        Assert.Equal(MemberStatus.Active, member.Status);
        Assert.Equal(userId, member.UserId);
        Assert.NotNull(member.JoinedAt);
    }

    [Fact]
    public void Error_AcceptInvite_Empty_UserId()
    {
        var member = TenantMemberBuilder.Build(status: MemberStatus.Invited);

        var exception = Assert.Throws<ErrorOnValidationException>(() =>
            member.AcceptInvite(Guid.Empty));

        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.USER_ID_REQUIRED, message);
    }

    [Fact]
    public void Error_AcceptInvite_When_Not_Invited()
    {
        var member = TenantMemberBuilder.Build(status: MemberStatus.Active);

        var exception = Assert.Throws<ErrorOnValidationException>(() =>
            member.AcceptInvite(Guid.NewGuid()));

        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.MEMBER_STATUS_INVALID, message);
    }

    [Fact]
    public void Success_UpdateRole()
    {
        var member = TenantMemberBuilder.Build(role: TenantRole.Member);

        member.UpdateRole(TenantRole.Admin);

        Assert.Equal(TenantRole.Admin, member.Role);
    }

    [Fact]
    public void Success_Suspend_And_Activate()
    {
        var member = TenantMemberBuilder.Build(status: MemberStatus.Active);

        member.Suspend();
        Assert.Equal(MemberStatus.Suspended, member.Status);

        member.Activate();
        Assert.Equal(MemberStatus.Active, member.Status);
    }

    [Fact]
    public void Error_Suspend_When_Already_Suspended()
    {
        var member = TenantMemberBuilder.Build(status: MemberStatus.Suspended);

        var exception = Assert.Throws<ErrorOnValidationException>(() => member.Suspend());
        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.MEMBER_ALREADY_SUSPENDED, message);
    }

    [Fact]
    public void Error_Activate_When_Already_Active()
    {
        var member = TenantMemberBuilder.Build(status: MemberStatus.Active);

        var exception = Assert.Throws<ErrorOnValidationException>(() => member.Activate());
        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.MEMBER_ALREADY_ACTIVE, message);
    }
}
