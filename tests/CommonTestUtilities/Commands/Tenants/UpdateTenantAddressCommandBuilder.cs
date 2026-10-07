using Bogus;
using CommonTestUtilities.DTOs.Workspaces;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.UpdateWorkspaceAddress;

namespace CommonTestUtilities.Commands.Tenants;

public class UpdateWorkspaceAddressCommandBuilder
{
    public static UpdateWorkspaceAddressCommand Build(
        Guid? id = null,
        bool isWorkspaceAddressNull = false,
        bool isStreetEmpty = false,
        bool isNumberEmpty = false,
        bool isNeighborhoodEmpty = false,
        bool isCityEmpty = false,
        bool isStateEmpty = false,
        bool isZipCodeEmpty = false
    )
    {
        var faker = new Faker<UpdateWorkspaceAddressCommand>()
            .CustomInstantiator(f => new UpdateWorkspaceAddressCommand(
                Id: id ?? Guid.NewGuid(),
                WorkspaceAddress: isWorkspaceAddressNull ? null! : WorkspaceAddressDtoBuilder.Build(
                    isStreetEmpty: isStreetEmpty,
                    isNumberEmpty: isNumberEmpty,
                    isNeighborhoodEmpty: isNeighborhoodEmpty,
                    isCityEmpty: isCityEmpty,
                    isStateEmpty: isStateEmpty,
                    isZipCodeEmpty: isZipCodeEmpty
                )
            ));

        return faker.Generate();
    }
}
