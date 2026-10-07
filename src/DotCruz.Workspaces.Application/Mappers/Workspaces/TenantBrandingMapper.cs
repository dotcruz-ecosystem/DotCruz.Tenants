using DotCruz.Workspaces.Application.DTOs.Workspaces;
using DotCruz.Workspaces.Domain.ValueObjects.Workspaces;

namespace DotCruz.Workspaces.Application.Mappers.Workspaces;

public static class WorkspaceBrandingMapper
{
    public static WorkspaceBrandingDto? ToDto(this WorkspaceBranding? WorkspaceBranding)
    {
        if (WorkspaceBranding is null)
            return null;

        return new WorkspaceBrandingDto(
            WorkspaceBranding.LogoUrl,
            WorkspaceBranding.HeaderBackgroundColor,
            WorkspaceBranding.Website,
            WorkspaceBranding.UnsubscribeUrl
        );
    }

    public static WorkspaceBranding ToDomain(this WorkspaceBrandingDto WorkspaceBrandingDto)
    {
        return WorkspaceBranding.Create(
            WorkspaceBrandingDto.LogoUrl,
            WorkspaceBrandingDto.HeaderBackgroundColor,
            WorkspaceBrandingDto.Website,
            WorkspaceBrandingDto.UnsubscribeUrl
        );
    }
}
