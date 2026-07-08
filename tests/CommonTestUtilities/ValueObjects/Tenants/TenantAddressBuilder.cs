using Bogus;
using CommonTestUtilities.ValueObjects.Location;
using DotCruz.Tenants.Domain.ValueObjects.Location;
using DotCruz.Tenants.Domain.ValueObjects.Tenants;

namespace CommonTestUtilities.ValueObjects.Tenants;

public class TenantAddressBuilder
{
    public static TenantAddress Build(
        bool isStreetEmpty = false,
        bool isNumberEmpty = false,
        bool isNeighborhoodEmpty = false,
        bool isCityEmpty = false,
        bool isStateNull = false,
        bool isZipCodeNull = false)
    {
        var faker = new Faker();

        var street = faker.Address.StreetName();
        var number = faker.Address.BuildingNumber();
        var complement = faker.Address.SecondaryAddress();
        var neighborhood = faker.Address.County();
        var city = faker.Address.City();
        var state = StateBuilder.Build();
        var zipCode = ZipCodeBuilder.Build();

        if (isStreetEmpty)
            street = string.Empty;

        if (isNumberEmpty)
            number = string.Empty;

        if (isNeighborhoodEmpty)
            neighborhood = string.Empty;

        if (isCityEmpty)
            city = string.Empty;

        return TenantAddress.Create(
            street,
            number,
            complement,
            neighborhood,
            city,
            isStateNull ? null! : state,
            isZipCodeNull ? null! : zipCode
        );
    }
}
