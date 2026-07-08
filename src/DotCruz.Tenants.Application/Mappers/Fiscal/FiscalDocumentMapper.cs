using DotCruz.Tenants.Application.DTOs.Fiscal;
using DotCruz.Tenants.Domain.ValueObjects.Fiscal;

namespace DotCruz.Tenants.Application.Mappers.Fiscal;

public static class FiscalDocumentMapper
{
    public static FiscalDocumentDto ToDto(this FiscalDocument fiscalDocument)
    {
        return new FiscalDocumentDto(
            fiscalDocument.Number,
            fiscalDocument.Type
        );
    }

    public static FiscalDocument ToDomain(this FiscalDocumentDto fiscalDocumentDto)
    {
        return FiscalDocument.Create(
            fiscalDocumentDto.DocumentNumber,
            fiscalDocumentDto.DocumentType
        );
    }
}
