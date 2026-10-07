using DotCruz.Workspaces.Application.DTOs.Workspaces;
using DotCruz.Workspaces.Application.Mappers.Fiscal;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.CreateTenant;
using DotCruz.Workspaces.Domain.Entities.Workspaces;
using DotCruz.Workspaces.Domain.ValueObjects.Identity;
using DotCruz.Workspaces.Domain.ValueObjects.Workspaces;

namespace DotCruz.Workspaces.Application.Mappers.Workspaces;

public static class WorkspaceMapper
{

    public static Workspace ToDomain(this CreateTenantCommand tenantCommand)
    {
        return Workspace.Create(
            Name.Create(tenantCommand.Name),
            WorkspaceSlug.Create(tenantCommand.Slug),
            tenantCommand.TenantDocument.ToDomain(),
            tenantCommand.WorkspaceContact.ToDomain(),
            tenantCommand.WorkspaceAddress.ToDomain(),
            adminEmail: DotCruz.Workspaces.Domain.ValueObjects.Communication.Email.Create(tenantCommand.TenantAdminUser.AdminEmail)
        );
    }

    public static WorkspaceDto ToDto(this Workspace Workspace)
    {
        return new WorkspaceDto(
            Workspace.Id,
            Workspace.Name.Value,
            Workspace.Slug.Value,
            Workspace.Document != null ? Workspace.Document.ToDto() : null,
            Workspace.Type.ToString(),
            Workspace.Status.ToString(),
            Workspace.Contact != null ? Workspace.Contact.ToDto() : null,
            Workspace.Address != null ? Workspace.Address.ToDto() : null,
            Workspace.Subscription.ToDto(),
            Workspace.Branding.ToDto(),
            Workspace.SuspensionReason?.Value,
            Workspace.CreatedAt,
            Workspace.UpdatedAt
        );
    }

    public static WorkspaceSummaryDto ToSummaryDto(this Workspace Workspace)
    {
        return new WorkspaceSummaryDto(
            Workspace.Id,
            Workspace.Name.Value,
            Workspace.Slug.Value,
            Workspace.Status.ToString(),
            Workspace.Type.ToString(),
            Workspace.Subscription.Plan.ToString()
        );
    }
}
