namespace DotCruz.Tenants.Domain.ValueObjects.Tenants;

public record class TenantBranding
{
    public string LogoUrl { get; init; } = string.Empty;
    public string HeaderBackgroundColor { get; init; } = string.Empty;
    public string Website { get; init; } = string.Empty;
    public string UnsubscribeUrl { get; init; } = string.Empty;

    private TenantBranding() { }

    private TenantBranding(
        string logoUrl,
        string headerBackgroundColor,
        string website,
        string unsubscribeUrl)
    {
        LogoUrl = logoUrl;
        HeaderBackgroundColor = headerBackgroundColor;
        Website = website;
        UnsubscribeUrl = unsubscribeUrl;
    }

    public static TenantBranding Create(
        string logoUrl,
        string headerBackgroundColor,
        string website,
        string unsubscribeUrl)
    {
        return new TenantBranding(
            logoUrl?.Trim() ?? string.Empty,
            headerBackgroundColor?.Trim() ?? string.Empty,
            website?.Trim() ?? string.Empty,
            unsubscribeUrl?.Trim() ?? string.Empty);
    }
}
