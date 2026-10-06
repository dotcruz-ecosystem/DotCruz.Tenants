using DotCruz.Tenants.Domain.Entities.Base;
using DotCruz.Tenants.Domain.Enums.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using DotCruz.Tenants.Domain.ValueObjects.Communication;

namespace DotCruz.Tenants.Domain.Entities.Tenants;

public class TenantMember : BaseEntity
{
    public Guid TenantId { get; private init; }
    public Guid? UserId { get; private set; }
    public Email Email { get; private set; } = null!;
    public TenantRole Role { get; private set; }
    public MemberStatus Status { get; private set; }
    public DateTimeOffset InvitedAt { get; private init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? JoinedAt { get; private set; }

    public virtual Tenant Tenant { get; private init; } = null!;

    private TenantMember() { }

    public static TenantMember Create(
        Guid tenantId,
        Email email,
        TenantRole role = TenantRole.Member,
        Guid? userId = null,
        MemberStatus status = MemberStatus.Invited)
    {
        if (tenantId == Guid.Empty)
            throw new ErrorOnValidationException(ResourceMessagesException.TENANT_ID_REQUIRED);

        ArgumentNullException.ThrowIfNull(email);

        return new TenantMember
        {
            TenantId = tenantId,
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

    public void UpdateRole(TenantRole newRole)
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
