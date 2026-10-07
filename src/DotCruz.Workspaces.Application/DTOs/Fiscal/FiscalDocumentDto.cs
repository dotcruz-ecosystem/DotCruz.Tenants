using DotCruz.Workspaces.Domain.Enums.Workspaces;

namespace DotCruz.Workspaces.Application.DTOs.Fiscal;

public sealed record FiscalDocumentDto(
    string DocumentNumber,
    DocumentType DocumentType
);
