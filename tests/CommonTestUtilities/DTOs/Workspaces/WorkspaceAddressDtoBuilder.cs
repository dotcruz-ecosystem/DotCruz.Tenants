using Bogus;
using DotCruz.Workspaces.Application.DTOs.Workspaces;

namespace CommonTestUtilities.DTOs.Workspaces;

public class WorkspaceAddressDtoBuilder
{
    public static WorkspaceAddressDto Build(
        bool isStreetEmpty = false,
        bool isNumberEmpty = false,
        bool isNeighborhoodEmpty = false,
        bool isCityEmpty = false,
        bool isStateEmpty = false,
        bool isZipCodeEmpty = false
    )
    {
        var faker = new Faker<WorkspaceAddressDto>("pt_BR")
            .CustomInstantiator(f => new WorkspaceAddressDto(
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
