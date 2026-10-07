namespace DotCruz.Workspaces.Application.DTOs.Workspaces;

public sealed record WorkspaceSummaryDto(
    Guid Id,
    string Name,
    string Slug,
    string Status,
    string Type,
    string Plan
);
