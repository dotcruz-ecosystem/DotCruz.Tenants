using DotCruz.Tenants.Application.DTOs.Tenants;
using DotCruz.Tenants.Application.Mappers.Fiscal;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.CreateTenant;
using DotCruz.Tenants.Domain.Entities.Tenants;
using DotCruz.Tenants.Domain.ValueObjects.Identity;
using DotCruz.Tenants.Domain.ValueObjects.Tenants;

namespace DotCruz.Tenants.Application.Mappers.Tenants;

public static class TenantMapper
{

    public static Tenant ToDomain(this CreateTenantCommand tenantCommand)
    {
        return Tenant.Create(
            Name.Create(tenantCommand.Name),
            TenantSlug.Create(tenantCommand.Slug),
            tenantCommand.TenantDocument.ToDomain(),
            tenantCommand.TenantContact.ToDomain(),
            tenantCommand.TenantAddress.ToDomain()
        );
    }

    public static TenantDto ToDto(this Tenant tenant)
    {
        return new TenantDto(
            tenant.Id,
            tenant.Name.Value,
            tenant.Slug.Value,
            tenant.Document.ToDto(),
            tenant.Type.ToString(),
            tenant.Status.ToString(),
            tenant.Contact.ToDto(),
            tenant.Address.ToDto(),
            tenant.Subscription.ToDto(),
            tenant.Branding.ToDto(),
            tenant.SuspensionReason?.Value,
            tenant.CreatedAt,
            tenant.UpdatedAt
        );
    }

    public static TenantSummaryDto ToSummaryDto(this Tenant tenant)
    {
        return new TenantSummaryDto(
            tenant.Id,
            tenant.Name.Value,
            tenant.Slug.Value,
            tenant.Status.ToString(),
            tenant.Type.ToString(),
            tenant.Subscription.Plan.ToString()
        );
    }
}
