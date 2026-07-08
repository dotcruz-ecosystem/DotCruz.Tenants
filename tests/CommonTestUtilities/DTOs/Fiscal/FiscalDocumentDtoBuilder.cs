using Bogus;
using Bogus.Extensions.Brazil;
using DotCruz.Tenants.Application.DTOs.Fiscal;
using DotCruz.Tenants.Domain.Enums.Tenants;

namespace CommonTestUtilities.DTOs.Fiscal;

public class FiscalDocumentDtoBuilder
{
    public static FiscalDocumentDto Build(
        DocumentType? type = null,
        bool isDocumentEmpty = false,
        bool isTypeInvalid = false
    )
    {
        var faker = new Faker("pt_BR");

        type ??= faker.PickRandom<DocumentType>();

        string document = type switch
        {
            DocumentType.CPF => faker.Person.Cpf(),
            DocumentType.CNPJ => faker.Company.Cnpj(),
            _ => faker.Random.String2(11, "0123456789")
        };

        if (isDocumentEmpty)
            document = string.Empty;

        if (isTypeInvalid)
            type = (DocumentType)999;

        return new FiscalDocumentDto(
            DocumentNumber: document,
            DocumentType: type.Value
        );
    }
}
