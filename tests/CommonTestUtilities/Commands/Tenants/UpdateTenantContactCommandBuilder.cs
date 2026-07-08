using Bogus;
using CommonTestUtilities.DTOs.Tenants;
using DotCruz.Tenants.Application.UseCases.Tenants.UpdateTenantContact;

namespace CommonTestUtilities.Commands.Tenants;

public class UpdateTenantContactCommandBuilder
{
    public static UpdateTenantContactCommand Build(
        Guid? id = null,
        bool isTenantContactNull = false,
        bool isEmailEmpty = false,
        bool isCountryCodeInvalid = false,
        bool isPhoneEmpty = false
    )
    {
        var faker = new Faker<UpdateTenantContactCommand>()
            .CustomInstantiator(f => new UpdateTenantContactCommand(
                Id: id ?? Guid.NewGuid(),
                TenantContact: isTenantContactNull ? null! : TenantContactDtoBuilder.Build(
                    isEmailEmpty: isEmailEmpty,
                    isCountryCodeInvalid: isCountryCodeInvalid,
                    isPhoneEmpty: isPhoneEmpty
                )
            ));

        return faker.Generate();
    }
}
