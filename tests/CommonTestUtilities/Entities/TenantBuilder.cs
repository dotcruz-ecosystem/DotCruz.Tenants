using CommonTestUtilities.ValueObjects.Fiscal;
using CommonTestUtilities.ValueObjects.Identity;
using CommonTestUtilities.ValueObjects.Tenants;
using DotCruz.Tenants.Domain.Entities.Tenants;
using DotCruz.Tenants.Domain.ValueObjects.Tenants;

namespace CommonTestUtilities.Entities;

public class TenantBuilder
{
    public readonly Tenant _tenant;

    public TenantBuilder(TenantSubscription? subscription = null)
    {
        _tenant = Tenant.Create(
            name: NameBuilder.Build(),
            slug: TenantSlugBuilder.Build(),
            document: FiscalDocumentBuilder.Build(),
            contact: TenantContactBuilder.Build(),
            address: TenantAddressBuilder.Build(),
            subscription: subscription ?? TenantSubscriptionBuilder.Build()
        );
    }

    public TenantBuilder Activate()
    {
        _tenant.Activate();
        return this;
    }

    public TenantBuilder Suspend(SuspensionReason? reason = null, bool isSuspensionReasonNull = false)
    {
        _tenant.Suspend(
            isSuspensionReasonNull ? null! : (reason ?? SuspensionReasonBuilder.Build())
        );

        return this;
    }

    public TenantBuilder Deactivate()
    {
        _tenant.Deactivate();
        return this;
    }

    public TenantBuilder Terminate()
    {
        _tenant.Terminate();
        return this;
    }

    public TenantBuilder UpdateSubscription(TenantSubscription? subscription = null, bool isSubscriptionNull = false)
    {
        _tenant.UpdateSubscription(
            isSubscriptionNull ? null! : (subscription ?? TenantSubscriptionBuilder.Build())
        );

        return this;
    }

    public TenantBuilder UpdateContact(TenantContact? contact = null, bool isContactNull = false)
    {
        _tenant.UpdateContact(
            isContactNull ? null! : (contact ?? TenantContactBuilder.Build())
        );
        return this;
    }

    public TenantBuilder UpdateAddress(TenantAddress? address = null, bool isAddressNull = false)
    {
        _tenant.UpdateAddress(
            isAddressNull ? null! : (address ?? TenantAddressBuilder.Build())
        );
        return this;
    }

    public TenantBuilder MarkPastDue()
    {
        _tenant.MarkPastDue();
        return this;
    }

    public TenantBuilder WithMember(
        DotCruz.Tenants.Domain.ValueObjects.Communication.Email? email = null,
        DotCruz.Tenants.Domain.Enums.Tenants.TenantRole role = DotCruz.Tenants.Domain.Enums.Tenants.TenantRole.Member,
        Guid? userId = null,
        DotCruz.Tenants.Domain.Enums.Tenants.MemberStatus status = DotCruz.Tenants.Domain.Enums.Tenants.MemberStatus.Invited)
    {
        _tenant.AddMember(
            email ?? CommonTestUtilities.ValueObjects.Communication.EmailBuilder.Build(),
            role,
            userId,
            status
        );
        return this;
    }

    public TenantBuilder WithOwnerMember(
        DotCruz.Tenants.Domain.ValueObjects.Communication.Email? email = null,
        Guid? userId = null)
    {
        return WithMember(
            email, 
            DotCruz.Tenants.Domain.Enums.Tenants.TenantRole.Owner, 
            userId, 
            DotCruz.Tenants.Domain.Enums.Tenants.MemberStatus.Active
        );
    }

    public Tenant Build() => _tenant;
}
