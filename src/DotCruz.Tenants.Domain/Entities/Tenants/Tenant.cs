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
    public TenantStatus Status { get; private set; } = TenantStatus.PendingProvisioning;
    public TenantSlug Slug { get; private set; } = null!;
    public FiscalDocument Document { get; private set; } = null!;
    public TenantContact Contact { get; private set; } = null!;
    public TenantAddress Address { get; private set; } = null!;
    public TenantSubscription Subscription { get; private set; } = null!;
    public SuspensionReason? SuspensionReason { get; private set; }

    private Tenant() { }

    public static Tenant Create(
        Name name, 
        TenantSlug slug, 
        FiscalDocument document, 
        TenantContact contact, 
        TenantAddress address,
        TenantSubscription? subscription = null)
    {

        return new Tenant
        {
            Name = name,
            Slug = slug,
            Document = document,
            Contact = contact,
            Address = address,
            Subscription = subscription ?? TenantSubscription.CreateTrial(),
            Status = TenantStatus.PendingProvisioning
        };
    }

    public void Activate()
    {
        if (Status == TenantStatus.Terminated)
            throw new ErrorOnValidationException(ResourceMessagesException.TENANT_TERMINATED_CANNOT_REACTIVATE);

        Status = Subscription.Plan == PlanType.Trial ? TenantStatus.Trialing : TenantStatus.Active;
        SuspensionReason = null;
        Touch();
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
        Touch();
    }

    public void Deactivate()
    {
        if (Status == TenantStatus.Terminated)
            throw new ErrorOnValidationException(ResourceMessagesException.TENANT_TERMINATED_CANNOT_DEACTIVATE);

        Status = TenantStatus.Inactive;
        Touch();
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
        
        Touch();
    }

    public void UpdateContact(TenantContact newContact)
    {
        Contact = newContact ?? throw new ErrorOnValidationException(ResourceMessagesException.CONTACT_REQUIRED);
        Touch();
    }

    public void UpdateAddress(TenantAddress newAddress)
    {
        Address = newAddress ?? throw new ErrorOnValidationException(ResourceMessagesException.ADDRESS_REQUIRED);
        Touch();
    }

    public void MarkPastDue()
    {
        if (Status != TenantStatus.Active && Status != TenantStatus.Trialing)
            throw new ErrorOnValidationException(ResourceMessagesException.TENANT_PAST_DUE_INVALID_STATUS);

        Status = TenantStatus.PastDue;
        Touch();
    }
}
