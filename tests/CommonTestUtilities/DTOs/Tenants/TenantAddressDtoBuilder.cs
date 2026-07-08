using Bogus;
using DotCruz.Tenants.Application.DTOs.Tenants;

namespace CommonTestUtilities.DTOs.Tenants;

public class TenantAddressDtoBuilder
{
    public static TenantAddressDto Build(
        bool isStreetEmpty = false,
        bool isNumberEmpty = false,
        bool isNeighborhoodEmpty = false,
        bool isCityEmpty = false,
        bool isStateEmpty = false,
        bool isZipCodeEmpty = false
    )
    {
        var faker = new Faker<TenantAddressDto>("pt_BR")
            .CustomInstantiator(f => new TenantAddressDto(
                !isStreetEmpty ? f.Address.StreetName() : string.Empty,
                !isNumberEmpty ? f.Address.BuildingNumber() : string.Empty,
                f.Address.SecondaryAddress(),
                !isNeighborhoodEmpty ? f.Address.County() : string.Empty,
                !isCityEmpty ? f.Address.City() : string.Empty,
                !isStateEmpty ? f.Address.StateAbbr() : string.Empty,
                !isZipCodeEmpty ? f.Address.ZipCode() : string.Empty
            ));

        return faker.Generate();
    }
}
