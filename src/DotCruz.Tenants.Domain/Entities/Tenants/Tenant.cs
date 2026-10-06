using DotCruz.Tenants.Domain.Entities.Base;
using DotCruz.Tenants.Domain.Enums.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using DotCruz.Tenants.Domain.ValueObjects.Fiscal;
using DotCruz.Tenants.Domain.ValueObjects.Identity;
using DotCruz.Tenants.Domain.ValueObjects.Tenants;

namespace DotCruz.Tenants.Domain.Entities.Tenants;

public class Tenant : BaseEntity
{
    public Name Name { get; private set; } = null!;
    public TenantType Type { get; private set; } = TenantType.Personal;
    public TenantStatus Status { get; private set; } = TenantStatus.PendingProvisioning;
    public TenantSlug Slug { get; private set; } = null!;
    public FiscalDocument? Document { get; private set; }
    public TenantContact? Contact { get; private set; }
    public TenantAddress? Address { get; private set; }
    public TenantSubscription Subscription { get; private set; } = null!;
    public TenantBranding? Branding { get; private set; } = null!;
    public SuspensionReason? SuspensionReason { get; private set; }

    private readonly List<TenantMember> _members = [];
    public IReadOnlyCollection<TenantMember> Members => _members.AsReadOnly();

    private Tenant() { }

    public static Tenant CreateWorkspace(
        Name name,
        TenantSlug slug,
        TenantType type = TenantType.Personal,
        TenantSubscription? subscription = null,
        DotCruz.Tenants.Domain.ValueObjects.Communication.Email? ownerEmail = null,
        Guid? ownerUserId = null)
    {
        var tenant = new Tenant
        {
            Name = name,
            Type = type,
            Slug = slug,
            Subscription = subscription ?? TenantSubscription.CreateTrial(),
            Status = TenantStatus.Active
        };

        if (ownerEmail != null)
        {
            tenant.AddMember(ownerEmail, TenantRole.Owner, ownerUserId, MemberStatus.Active);
        }

        return tenant;
    }

    public static Tenant Create(
        Name name, 
        TenantSlug slug, 
        FiscalDocument document, 
        TenantContact contact, 
        TenantAddress address,
        TenantSubscription? subscription = null,
        TenantType type = TenantType.Personal,
        DotCruz.Tenants.Domain.ValueObjects.Communication.Email? adminEmail = null,
        Guid? adminUserId = null)
    {
        var tenant = new Tenant
        {
            Name = name,
            Type = type,
            Slug = slug,
            Document = document,
            Contact = contact,
            Address = address,
            Subscription = subscription ?? TenantSubscription.CreateTrial(),
            Status = TenantStatus.PendingProvisioning
        };

        if (adminEmail != null)
        {
            tenant.AddMember(adminEmail, TenantRole.Owner, adminUserId, MemberStatus.Active);
        }

        return tenant;
    }

    public TenantMember AddMember(
        DotCruz.Tenants.Domain.ValueObjects.Communication.Email email,
        TenantRole role = TenantRole.Member,
        Guid? userId = null,
        MemberStatus status = MemberStatus.Invited)
    {
        if (Status == TenantStatus.Terminated || Status == TenantStatus.Inactive)
            throw new ErrorOnValidationException(ResourceMessagesException.TENANT_INACTIVE_CANNOT_ADD_MEMBER);

        ArgumentNullException.ThrowIfNull(email);

        var existingMember = _members.FirstOrDefault(m => 
            m.DeletedAt == null && 
            string.Equals(m.Email.Value, email.Value, StringComparison.OrdinalIgnoreCase));

        if (existingMember != null)
            throw new ErrorOnValidationException(ResourceMessagesException.MEMBER_ALREADY_EXISTS);

        var activeCount = _members.Count(m => m.DeletedAt == null && m.Status != MemberStatus.Suspended);
        if (activeCount >= Subscription.Limits.MaxUsers)
            throw new ErrorOnValidationException(ResourceMessagesException.MAX_USERS_REACHED);

        var member = TenantMember.Create(Id, email, role, userId, status);
        _members.Add(member);
        Touch();

        return member;
    }

    public void RemoveMember(Guid memberId)
    {
        var member = _members.FirstOrDefault(m => m.Id == memberId && m.DeletedAt == null)
            ?? throw new NotFoundException(ResourceMessagesException.MEMBER_NOT_FOUND);

        if (member.Role == TenantRole.Owner)
        {
            var activeOwners = _members.Count(m => m.Role == TenantRole.Owner && m.DeletedAt == null);
            if (activeOwners <= 1)
                throw new ErrorOnValidationException(ResourceMessagesException.CANNOT_REMOVE_LAST_OWNER);
        }

        member.Delete();
        Touch();
    }

    public void Activate()
    {
        if (Status == TenantStatus.Terminated)
            throw new ErrorOnValidationException(ResourceMessagesException.TENANT_TERMINATED_CANNOT_REACTIVATE);

        Status = Subscription.Plan == PlanType.Trial ? TenantStatus.Trialing : TenantStatus.Active;
        SuspensionReason = null;
    }

    public void Suspend(SuspensionReason reason)
    {
        var errors = new List<string>();

        if (reason == null)
            errors.Add(ResourceMessagesException.SUSPENSION_REASON_EMPTY);

        if (Status == TenantStatus.Terminated)
            errors.Add(ResourceMessagesException.TENANT_TERMINATED_CANNOT_SUSPEND);

        if (errors.Count > 0)
            throw new ErrorOnValidationException(errors);

        Status = TenantStatus.Suspended;
        SuspensionReason = reason;
    }

    public void Deactivate()
    {
        if (Status == TenantStatus.Terminated)
            throw new ErrorOnValidationException(ResourceMessagesException.TENANT_TERMINATED_CANNOT_DEACTIVATE);

        Status = TenantStatus.Inactive;
    }

    public void Terminate()
    {
        Status = TenantStatus.Terminated;
        Delete();
    }

    public void UpdateSubscription(TenantSubscription newSubscription)
    {
        Subscription = newSubscription ?? throw new ErrorOnValidationException(ResourceMessagesException.SUBSCRIPTION_REQUIRED);
        
        if (Status == TenantStatus.Active || Status == TenantStatus.Trialing || Status == TenantStatus.PastDue)
            Status = newSubscription.Plan == PlanType.Trial ? TenantStatus.Trialing : TenantStatus.Active;
        
    }

    public void UpdateDocument(FiscalDocument newDocument)
    {
        Document = newDocument ?? throw new ErrorOnValidationException(ResourceMessagesException.DOCUMENT_REQUIRED);
    }

    public void UpdateContact(TenantContact newContact)
    {
        Contact = newContact ?? throw new ErrorOnValidationException(ResourceMessagesException.CONTACT_REQUIRED);
    }

    public void UpdateAddress(TenantAddress newAddress)
    {
        Address = newAddress ?? throw new ErrorOnValidationException(ResourceMessagesException.ADDRESS_REQUIRED);
    }

    public void UpdateBranding(TenantBranding newBranding)
    {
        Branding = newBranding ?? throw new ErrorOnValidationException(ResourceMessagesException.BRANDING_REQUIRED);
    }

    public void UpdateType(TenantType newType)
    {
        Type = newType;
    }

    public void MarkPastDue()
    {
        if (Status != TenantStatus.Active && Status != TenantStatus.Trialing)
            throw new ErrorOnValidationException(ResourceMessagesException.TENANT_PAST_DUE_INVALID_STATUS);

        Status = TenantStatus.PastDue;
    }
}
