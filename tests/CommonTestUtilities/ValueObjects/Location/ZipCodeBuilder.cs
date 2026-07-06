using Bogus;
using DotCruz.Tenants.Domain.ValueObjects.Location;

namespace CommonTestUtilities.ValueObjects.Location;

public class ZipCodeBuilder
{
    public static ZipCode Build(bool isEmpty = false)
    {
        var faker = new Faker("pt_BR");

        var zipcode = !isEmpty ? faker.Address.ZipCode() : string.Empty;

        return ZipCode.Create(zipcode);
    }
}
