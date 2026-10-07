namespace DotCruz.Workspaces.Application.DTOs.Workspaces;

public sealed record WorkspaceBrandingDto(
    string LogoUrl,
    string HeaderBackgroundColor,
    string Website,
    string UnsubscribeUrl
);
