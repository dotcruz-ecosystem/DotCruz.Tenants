using DotCruz.Workspaces.Domain.Entities.Base;
using DotCruz.Workspaces.Domain.Enums.Workspaces;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;
using DotCruz.Workspaces.Domain.ValueObjects.Communication;

namespace DotCruz.Workspaces.Domain.Entities.Workspaces;

public class WorkspaceMember : BaseEntity
{
    public Guid WorkspaceId { get; private init; }
    public Guid? UserId { get; private set; }
    public Email Email { get; private set; } = null!;
    public WorkspaceRole Role { get; private set; }
    public MemberStatus Status { get; private set; }
    public DateTimeOffset InvitedAt { get; private init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? JoinedAt { get; private set; }

    public virtual Workspace Workspace { get; private init; } = null!;

    private WorkspaceMember() { }

    public static WorkspaceMember Create(
        Guid WorkspaceId,
        Email email,
        WorkspaceRole role = WorkspaceRole.Member,
        Guid? userId = null,
        MemberStatus status = MemberStatus.Invited)
    {
        if (WorkspaceId == Guid.Empty)
            throw new ErrorOnValidationException(ResourceMessagesException.TENANT_ID_REQUIRED);

        ArgumentNullException.ThrowIfNull(email);

        return new WorkspaceMember
        {
            WorkspaceId = WorkspaceId,
            UserId = userId,
            Email = email,
            Role = role,
            Status = status,
            InvitedAt = DateTimeOffset.UtcNow,
            JoinedAt = status == MemberStatus.Active ? DateTimeOffset.UtcNow : null
        };
    }

    public void AcceptInvite(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ErrorOnValidationException(ResourceMessagesException.USER_ID_REQUIRED);

        if (Status != MemberStatus.Invited)
            throw new ErrorOnValidationException(ResourceMessagesException.MEMBER_STATUS_INVALID);

        UserId = userId;
        Status = MemberStatus.Active;
        JoinedAt = DateTimeOffset.UtcNow;
        Touch();
    }

    public void UpdateRole(WorkspaceRole newRole)
    {
        Role = newRole;
        Touch();
    }

    public void Suspend()
    {
        if (Status == MemberStatus.Suspended)
            throw new ErrorOnValidationException(ResourceMessagesException.MEMBER_ALREADY_SUSPENDED);

        Status = MemberStatus.Suspended;
        Touch();
    }

    public void Activate()
    {
        if (Status == MemberStatus.Active)
            throw new ErrorOnValidationException(ResourceMessagesException.MEMBER_ALREADY_ACTIVE);

        Status = MemberStatus.Active;
        Touch();
    }
}
