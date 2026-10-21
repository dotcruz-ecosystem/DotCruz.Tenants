using DotCruz.Workspaces.Domain.Entities.Base;
using DotCruz.Workspaces.Domain.Enums.Workspaces;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;
using DotCruz.Workspaces.Domain.ValueObjects.Fiscal;
using DotCruz.Workspaces.Domain.ValueObjects.Identity;
using DotCruz.Workspaces.Domain.ValueObjects.Workspaces;

namespace DotCruz.Workspaces.Domain.Entities.Workspaces;

public class Workspace : BaseEntity
{
    public Name Name { get; private set; } = null!;
    public WorkspaceType Type { get; private set; } = WorkspaceType.Personal;
    public WorkspaceStatus Status { get; private set; } = WorkspaceStatus.PendingProvisioning;
    public WorkspaceSlug Slug { get; private set; } = null!;
    public FiscalDocument? Document { get; private set; }
    public WorkspaceContact? Contact { get; private set; }
    public WorkspaceAddress? Address { get; private set; }
    public WorkspaceSubscription Subscription { get; private set; } = null!;
    public WorkspaceBranding? Branding { get; private set; } = null!;
    public SuspensionReason? SuspensionReason { get; private set; }

    private readonly List<WorkspaceMember> _members = [];
    public IReadOnlyCollection<WorkspaceMember> Members => _members.AsReadOnly();

    private Workspace() { }

    public static Workspace CreateWorkspace(
        Name name,
        WorkspaceSlug slug,
        WorkspaceType type = WorkspaceType.Personal,
        WorkspaceSubscription? subscription = null,
        DotCruz.Workspaces.Domain.ValueObjects.Communication.Email? ownerEmail = null,
        Guid? ownerUserId = null)
    {
        var Workspace = new Workspace
        {
            Name = name,
            Type = type,
            Slug = slug,
            Subscription = subscription ?? (type == WorkspaceType.Personal ? WorkspaceSubscription.CreateFree() : WorkspaceSubscription.CreateTrial()),
            Status = WorkspaceStatus.Active
        };

        if (ownerEmail != null)
        {
            Workspace.AddMember(ownerEmail, WorkspaceRole.Owner, ownerUserId, MemberStatus.Active);
        }

        return Workspace;
    }

    public static Workspace Create(
        Name name, 
        WorkspaceSlug slug, 
        FiscalDocument document, 
        WorkspaceContact contact, 
        WorkspaceAddress address,
        WorkspaceSubscription? subscription = null,
        WorkspaceType type = WorkspaceType.Enterprise,
        DotCruz.Workspaces.Domain.ValueObjects.Communication.Email? adminEmail = null,
        Guid? adminUserId = null)
    {
        var Workspace = new Workspace
        {
            Name = name,
            Type = type,
            Slug = slug,
            Document = document,
            Contact = contact,
            Address = address,
            Subscription = subscription ?? WorkspaceSubscription.CreateTrial(),
            Status = WorkspaceStatus.PendingProvisioning
        };

        if (adminEmail != null)
        {
            Workspace.AddMember(adminEmail, WorkspaceRole.Owner, adminUserId, MemberStatus.Active);
        }

        return Workspace;
    }

    public WorkspaceMember AddMember(
        DotCruz.Workspaces.Domain.ValueObjects.Communication.Email email,
        WorkspaceRole role = WorkspaceRole.Member,
        Guid? userId = null,
        MemberStatus status = MemberStatus.Invited)
    {
        if (Status == WorkspaceStatus.Terminated || Status == WorkspaceStatus.Inactive)
            throw new ErrorOnValidationException(ResourceMessagesException.TENANT_INACTIVE_CANNOT_ADD_MEMBER);

        ArgumentNullException.ThrowIfNull(email);

        var existingMember = _members.FirstOrDefault(m => 
            m.DeletedAt == null && 
            string.Equals(m.Email.Value, email.Value, StringComparison.OrdinalIgnoreCase));

        if (existingMember != null)
            throw new ErrorOnValidationException(ResourceMessagesException.MEMBER_ALREADY_EXISTS);

        var activeCount = _members.Count(m => m.DeletedAt == null && m.Status != MemberStatus.Suspended);
        if (Type != WorkspaceType.Personal && activeCount >= Subscription.Limits.MaxUsers)
            throw new ErrorOnValidationException(ResourceMessagesException.MAX_USERS_REACHED);

        var member = WorkspaceMember.Create(Id, email, role, userId, status);
        _members.Add(member);
        Touch();

        return member;
    }

    public void RemoveMember(Guid memberId)
    {
        var member = _members.FirstOrDefault(m => m.Id == memberId && m.DeletedAt == null)
            ?? throw new NotFoundException(ResourceMessagesException.MEMBER_NOT_FOUND);

        if (member.Role == WorkspaceRole.Owner)
        {
            var activeOwners = _members.Count(m => m.Role == WorkspaceRole.Owner && m.DeletedAt == null);
            if (activeOwners <= 1)
                throw new ErrorOnValidationException(ResourceMessagesException.CANNOT_REMOVE_LAST_OWNER);
        }

        member.Delete();
        Touch();
    }

    public void Activate()
    {
        if (Status == WorkspaceStatus.Terminated)
            throw new ErrorOnValidationException(ResourceMessagesException.TENANT_TERMINATED_CANNOT_REACTIVATE);

        Status = Subscription.Plan == PlanType.Trial ? WorkspaceStatus.Trialing : WorkspaceStatus.Active;
        SuspensionReason = null;
    }

    public void Suspend(SuspensionReason reason)
    {
        var errors = new List<string>();

        if (reason == null)
            errors.Add(ResourceMessagesException.SUSPENSION_REASON_EMPTY);

        if (Status == WorkspaceStatus.Terminated)
            errors.Add(ResourceMessagesException.TENANT_TERMINATED_CANNOT_SUSPEND);

        if (errors.Count > 0)
            throw new ErrorOnValidationException(errors);

        Status = WorkspaceStatus.Suspended;
        SuspensionReason = reason;
    }

    public void Deactivate()
    {
        if (Status == WorkspaceStatus.Terminated)
            throw new ErrorOnValidationException(ResourceMessagesException.TENANT_TERMINATED_CANNOT_DEACTIVATE);

        Status = WorkspaceStatus.Inactive;
    }

    public void Terminate()
    {
        Status = WorkspaceStatus.Terminated;
        Delete();
    }

    public void UpdateSubscription(WorkspaceSubscription newSubscription)
    {
        Subscription = newSubscription ?? throw new ErrorOnValidationException(ResourceMessagesException.SUBSCRIPTION_REQUIRED);
        
        if (Status == WorkspaceStatus.Active || Status == WorkspaceStatus.Trialing || Status == WorkspaceStatus.PastDue)
            Status = newSubscription.Plan == PlanType.Trial ? WorkspaceStatus.Trialing : WorkspaceStatus.Active;
        
    }

    public void UpdateDocument(FiscalDocument newDocument)
    {
        Document = newDocument ?? throw new ErrorOnValidationException(ResourceMessagesException.DOCUMENT_REQUIRED);
    }

    public void UpdateContact(WorkspaceContact newContact)
    {
        Contact = newContact ?? throw new ErrorOnValidationException(ResourceMessagesException.CONTACT_REQUIRED);
    }

    public void UpdateAddress(WorkspaceAddress newAddress)
    {
        Address = newAddress ?? throw new ErrorOnValidationException(ResourceMessagesException.ADDRESS_REQUIRED);
    }

    public void UpdateBranding(WorkspaceBranding newBranding)
    {
        Branding = newBranding ?? throw new ErrorOnValidationException(ResourceMessagesException.BRANDING_REQUIRED);
    }

    public void UpdateType(WorkspaceType newType)
    {
        Type = newType;
    }

    public void MarkPastDue()
    {
        if (Status != WorkspaceStatus.Active && Status != WorkspaceStatus.Trialing)
            throw new ErrorOnValidationException(ResourceMessagesException.TENANT_PAST_DUE_INVALID_STATUS);

        Status = WorkspaceStatus.PastDue;
    }
}
