namespace DotCruz.Workspaces.Domain.ValueObjects.Workspaces;

public record class WorkspaceBranding
{
    public string LogoUrl { get; init; } = string.Empty;
    public string HeaderBackgroundColor { get; init; } = string.Empty;
    public string Website { get; init; } = string.Empty;
    public string UnsubscribeUrl { get; init; } = string.Empty;

    private WorkspaceBranding() { }

    private WorkspaceBranding(
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

    public static WorkspaceBranding Create(
        string logoUrl,
        string headerBackgroundColor,
        string website,
        string unsubscribeUrl)
    {
        return new WorkspaceBranding(
            logoUrl?.Trim() ?? string.Empty,
            headerBackgroundColor?.Trim() ?? string.Empty,
            website?.Trim() ?? string.Empty,
            unsubscribeUrl?.Trim() ?? string.Empty);
    }
}
