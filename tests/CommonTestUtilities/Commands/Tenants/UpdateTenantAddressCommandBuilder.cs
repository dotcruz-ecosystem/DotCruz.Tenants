using Bogus;
using CommonTestUtilities.DTOs.Tenants;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.UpdateTenantAddress;

namespace CommonTestUtilities.Commands.Tenants;

public class UpdateTenantAddressCommandBuilder
{
    public static UpdateTenantAddressCommand Build(
        Guid? id = null,
        bool isTenantAddressNull = false,
        bool isStreetEmpty = false,
        bool isNumberEmpty = false,
        bool isNeighborhoodEmpty = false,
        bool isCityEmpty = false,
        bool isStateEmpty = false,
        bool isZipCodeEmpty = false
    )
    {
        var faker = new Faker<UpdateTenantAddressCommand>()
            .CustomInstantiator(f => new UpdateTenantAddressCommand(
                Id: id ?? Guid.NewGuid(),
                TenantAddress: isTenantAddressNull ? null! : TenantAddressDtoBuilder.Build(
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
