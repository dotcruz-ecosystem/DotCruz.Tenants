using Bogus;
using DotCruz.Tenants.Domain.ValueObjects.Communication;

namespace CommonTestUtilities.ValueObjects.Communication;

public class PhoneNumberBuilder
{
    public static PhoneNumber Build(
        bool isCountryCodeInvalid = false, 
        bool isPhoneEmpty = false, 
        bool isPhoneInvalid = false
    )
    {
        var faker = new Faker();

        var countryCode = !isCountryCodeInvalid ? faker.Random.Int(min: 1, max: 999) : 0;
        var nationalNumber = !isPhoneEmpty ? faker.Phone.PhoneNumber() : string.Empty;

        if (isPhoneInvalid)
            nationalNumber = faker.Lorem.Sentence();

        return PhoneNumber.Create(countryCode, nationalNumber);
    }
}