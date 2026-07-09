using Bogus;
using CommonTestUtilities.DTOs.Fiscal;
using CommonTestUtilities.DTOs.Tenants;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.CreateTenant;
using DotCruz.Tenants.Domain.Enums.Tenants;

namespace CommonTestUtilities.Commands.Tenants;

public class CreateTenantCommandBuilder
{
    public static CreateTenantCommand Build(
        bool isNameEmpty = false,
        bool isSlugEmpty = false,

        bool isTenantDocumentNull = false,
        DocumentType? documentType = null,
        bool isDocumentEmpty = false,
        bool isTypeInvalid = false,

        bool isTenantContactNull = false,
        bool isEmailEmpty = false,
        bool isCountryCodeInvalid = false,
        bool isPhoneEmpty = false,

        bool isTenantAddressNull = false,
        bool isStreetEmpty = false,
        bool isNumberEmpty = false,
        bool isNeighborhoodEmpty = false,
        bool isCityEmpty = false,
        bool isStateEmpty = false,
        bool isZipCodeEmpty = false,

        bool isTenantAdminUserNull = false,
        bool isAdminUserNameEmpty = false,
        bool isAdminUserNameInvalid = false,
        bool isAdminUserEmailEmpty = false,
        bool isAdminUserEmailInvalid = false
    )
    {
        var faker = new Faker<CreateTenantCommand>()
            .CustomInstantiator(f => new CreateTenantCommand(
                Name: !isNameEmpty ? f.Company.CompanyName() : string.Empty,
                Slug: !isSlugEmpty ? f.Internet.DomainWord() + "xyz" : string.Empty,

                TenantDocument: isTenantDocumentNull ? null! : FiscalDocumentDtoBuilder.Build(
                    type: documentType, 
                    isDocumentEmpty: isDocumentEmpty, 
                    isTypeInvalid: isTypeInvalid
                ),

                TenantContact: isTenantContactNull ? null! : TenantContactDtoBuilder.Build(
                    isEmailEmpty: isEmailEmpty, 
                    isCountryCodeInvalid: isCountryCodeInvalid, 
                    isPhoneEmpty: isPhoneEmpty
                ),

                TenantAddress: isTenantAddressNull ? null! : TenantAddressDtoBuilder.Build(
                    isStreetEmpty: isStreetEmpty,
                    isNumberEmpty: isNumberEmpty,
                    isNeighborhoodEmpty: isNeighborhoodEmpty,
                    isCityEmpty: isCityEmpty,
                    isStateEmpty: isStateEmpty,
                    isZipCodeEmpty: isZipCodeEmpty
                ),

                TenantAdminUser: isTenantAdminUserNull ? null! : TenantAdminUserDtoBuilder.Build(
                    isAdminNameEmpty: isAdminUserNameEmpty,
                    isAdminNameInvalid: isAdminUserNameInvalid,
                    isAdminEmailEmpty: isAdminUserEmailEmpty,
                    isAdminEmailInvalid: isAdminUserEmailInvalid
                )
            ));

        return faker.Generate();
    }
}
