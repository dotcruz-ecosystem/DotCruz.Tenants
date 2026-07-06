using Bogus;
using Bogus.Extensions.Brazil;
using DotCruz.Tenants.Domain.Enums.Tenants;
using DotCruz.Tenants.Domain.ValueObjects.Fiscal;

namespace CommonTestUtilities.ValueObjects.Fiscal;

public class FiscalDocumentBuilder
{
    public static FiscalDocument Build(
        bool isDocumentEmpty = false,
        bool isTypeInvalid = false,
        bool isDocumentInvalid = false
    )
    {
        var faker = new Faker("pt_BR");

        var type = faker.PickRandom<DocumentType>();

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

        if (isDocumentInvalid)
            document = faker.Random.String2(faker.Random.Int(1, 10));

        return FiscalDocument.Create(document, type);
    }
}
