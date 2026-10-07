using Bogus;
using DotCruz.Workspaces.Application.DTOs.Workspaces;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace CommonTestUtilities.DTOs.Workspaces;

public class WorkspaceContactDtoBuilder
{
    public static WorkspaceContactDto Build(
        bool isEmailEmpty = false,
        bool isCountryCodeInvalid = false,
        bool isPhoneEmpty = false
    )
    {
        var faker = new Faker("pt_BR");

        var email = !isEmailEmpty ? faker.Internet.Email() : string.Empty;

        var countryCode = !isCountryCodeInvalid ? faker.Random.Int(min: 1, max: 999) : 0;
        var nationalNumber = !isPhoneEmpty ? faker.Phone.PhoneNumber("###########") : string.Empty;

        return new WorkspaceContactDto(
            Email: email,
            PhoneCountryCode: countryCode,
            PhoneNationalNumber: nationalNumber
        );
    }
}
