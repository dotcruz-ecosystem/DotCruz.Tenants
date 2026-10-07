using Bogus;
using CommonTestUtilities.ValueObjects.Location;
using DotCruz.Workspaces.Domain.ValueObjects.Location;
using DotCruz.Workspaces.Domain.ValueObjects.Workspaces;

namespace CommonTestUtilities.ValueObjects.Workspaces;

public class WorkspaceAddressBuilder
{
    public static WorkspaceAddress Build(
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

        return WorkspaceAddress.Create(
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
