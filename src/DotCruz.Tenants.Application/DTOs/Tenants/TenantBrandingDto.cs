namespace DotCruz.Tenants.Application.DTOs.Tenants;

public sealed record TenantBrandingDto(
    string LogoUrl,
    string HeaderBackgroundColor,
    string Website,
    string UnsubscribeUrl
);
