using CommonTestUtilities.ValueObjects.Fiscal;
using CommonTestUtilities.ValueObjects.Identity;
using CommonTestUtilities.ValueObjects.Workspaces;
using DotCruz.Workspaces.Domain.Entities.Workspaces;
using DotCruz.Workspaces.Domain.ValueObjects.Workspaces;

namespace CommonTestUtilities.Entities;

public class WorkspaceBuilder
{
    public readonly Workspace _tenant;

    public WorkspaceBuilder(WorkspaceSubscription? subscription = null)
    {
        _tenant = Workspace.Create(
            name: NameBuilder.Build(),
            slug: WorkspaceSlugBuilder.Build(),
            document: FiscalDocumentBuilder.Build(),
            contact: WorkspaceContactBuilder.Build(),
            address: WorkspaceAddressBuilder.Build(),
            subscription: subscription ?? WorkspaceSubscriptionBuilder.Build()
        );
    }

    public WorkspaceBuilder Activate()
    {
        _tenant.Activate();
        return this;
    }

    public WorkspaceBuilder Suspend(SuspensionReason? reason = null, bool isSuspensionReasonNull = false)
    {
        _tenant.Suspend(
            isSuspensionReasonNull ? null! : (reason ?? SuspensionReasonBuilder.Build())
        );

        return this;
    }

    public WorkspaceBuilder Deactivate()
    {
        _tenant.Deactivate();
        return this;
    }

    public WorkspaceBuilder Terminate()
    {
        _tenant.Terminate();
        return this;
    }

    public WorkspaceBuilder UpdateSubscription(WorkspaceSubscription? subscription = null, bool isSubscriptionNull = false)
    {
        _tenant.UpdateSubscription(
            isSubscriptionNull ? null! : (subscription ?? WorkspaceSubscriptionBuilder.Build())
        );

        return this;
    }

    public WorkspaceBuilder UpdateContact(WorkspaceContact? contact = null, bool isContactNull = false)
    {
        _tenant.UpdateContact(
            isContactNull ? null! : (contact ?? WorkspaceContactBuilder.Build())
        );
        return this;
    }

    public WorkspaceBuilder UpdateAddress(WorkspaceAddress? address = null, bool isAddressNull = false)
    {
        _tenant.UpdateAddress(
            isAddressNull ? null! : (address ?? WorkspaceAddressBuilder.Build())
        );
        return this;
    }

    public WorkspaceBuilder MarkPastDue()
    {
        _tenant.MarkPastDue();
        return this;
    }

    public WorkspaceBuilder WithMember(
        DotCruz.Workspaces.Domain.ValueObjects.Communication.Email? email = null,
        DotCruz.Workspaces.Domain.Enums.Workspaces.WorkspaceRole role = DotCruz.Workspaces.Domain.Enums.Workspaces.WorkspaceRole.Member,
        Guid? userId = null,
        DotCruz.Workspaces.Domain.Enums.Workspaces.MemberStatus status = DotCruz.Workspaces.Domain.Enums.Workspaces.MemberStatus.Invited)
    {
        _tenant.AddMember(
            email ?? CommonTestUtilities.ValueObjects.Communication.EmailBuilder.Build(),
            role,
            userId,
            status
        );
        return this;
    }

    public WorkspaceBuilder WithOwnerMember(
        DotCruz.Workspaces.Domain.ValueObjects.Communication.Email? email = null,
        Guid? userId = null)
    {
        return WithMember(
            email, 
            DotCruz.Workspaces.Domain.Enums.Workspaces.WorkspaceRole.Owner, 
            userId, 
            DotCruz.Workspaces.Domain.Enums.Workspaces.MemberStatus.Active
        );
    }

    public Workspace Build() => _tenant;
}
