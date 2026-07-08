using DotCruz.Tenants.Domain.Enums.Tenants;

namespace DotCruz.Tenants.Application.DTOs.Fiscal;

public sealed record FiscalDocumentDto(
    string DocumentNumber,
    DocumentType DocumentType
);
