using DotCruz.Tenants.Application.DTOs.Tenants;
using DotCruz.Tenants.Domain.ValueObjects.Location;
using DotCruz.Tenants.Domain.ValueObjects.Tenants;

namespace DotCruz.Tenants.Application.Mappers.Tenants;

public static class TenantAddressMapper
{
    public static TenantAddressDto ToDto(this TenantAddress tenantAddress)
    {
        return new TenantAddressDto(
            tenantAddress.Street,
            tenantAddress.Number,
            tenantAddress.Complement,
            tenantAddress.Neighborhood,
            tenantAddress.City,
            tenantAddress.State,
            tenantAddress.ZipCode
        );
    }
    public static TenantAddress ToDomain(this TenantAddressDto tenantAddressDto)
    {
        return TenantAddress.Create(
            tenantAddressDto.Street,
            tenantAddressDto.Number,
            tenantAddressDto.Complement,
            tenantAddressDto.Neighborhood,
            tenantAddressDto.City,
            State.Create(tenantAddressDto.State),
            ZipCode.Create(tenantAddressDto.ZipCode)
        );
    }
}
