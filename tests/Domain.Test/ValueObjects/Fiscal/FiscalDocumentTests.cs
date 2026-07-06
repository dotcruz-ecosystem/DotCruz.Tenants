using CommonTestUtilities.ValueObjects.Fiscal;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using DotCruz.Tenants.Domain.ValueObjects.Fiscal;

namespace Domain.Test.ValueObjects.Fiscal;

public class FiscalDocumentTests
{
    [Fact]
    public void Success()
    {
        var fiscalDocument = FiscalDocumentBuilder.Build();

        Assert.NotNull(fiscalDocument);
        Assert.NotEmpty(fiscalDocument.Number);
        Assert.NotEmpty(fiscalDocument.FormattedValue);
    }

    [Fact]
    public void Error_Empty_Document()
    {
        static FiscalDocument act() => FiscalDocumentBuilder.Build(isDocumentEmpty: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.DOCUMENT_EMPTY, exceptionMessage);
    }

    [Fact]
    public void Error_Invalid_Type()
    {
        static FiscalDocument act() => FiscalDocumentBuilder.Build(isTypeInvalid: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.DOCUMENT_INVALID, exceptionMessage);
    }

    [Fact]
    public void Error_Invalid_Document()
    {
        static FiscalDocument act() => FiscalDocumentBuilder.Build(isDocumentInvalid: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.DOCUMENT_INVALID, exceptionMessage);
    }
}
