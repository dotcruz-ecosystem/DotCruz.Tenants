using DotCruz.Tenants.Application.DTOs.Tenants;
using DotCruz.Tenants.Domain.ValueObjects.Tenants;

namespace DotCruz.Tenants.Application.Mappers.Tenants;

public static class TenantBrandingMapper
{
    public static TenantBrandingDto ToDto(this TenantBranding tenantBranding)
    {
        return new TenantBrandingDto(
            tenantBranding.LogoUrl,
            tenantBranding.HeaderBackgroundColor,
            tenantBranding.Website,
            tenantBranding.UnsubscribeUrl
        );
    }

    public static TenantBranding ToDomain(this TenantBrandingDto tenantBrandingDto)
    {
        return TenantBranding.Create(
            tenantBrandingDto.LogoUrl,
            tenantBrandingDto.HeaderBackgroundColor,
            tenantBrandingDto.Website,
            tenantBrandingDto.UnsubscribeUrl
        );
    }
}
