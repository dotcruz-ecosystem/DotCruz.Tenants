using DotCruz.Workspaces.Application.DTOs.Fiscal;

namespace DotCruz.Workspaces.Application.DTOs.Workspaces;

public sealed record WorkspaceDto(
    Guid Id,
    string Name,
    string Slug,
    FiscalDocumentDto? TenantDocument,
    string Type,
    string Status,
    WorkspaceContactDto? WorkspaceContact,
    WorkspaceAddressDto? WorkspaceAddress,
    WorkspaceSubscriptionDto WorkspaceSubscription,
    WorkspaceBrandingDto? WorkspaceBranding,
    string? SuspensionReason,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
);
