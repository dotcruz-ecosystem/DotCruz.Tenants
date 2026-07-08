using DotCruz.Tenants.Application.DTOs.Tenants;
using DotCruz.Tenants.Domain.ValueObjects.Communication;
using DotCruz.Tenants.Domain.ValueObjects.Tenants;

namespace DotCruz.Tenants.Application.Mappers.Tenants;

public static class TenantContactMapper
{
    public static TenantContactDto ToDto(this TenantContact tenantContact)
    {
        return new TenantContactDto(
            tenantContact.Email,
            tenantContact.Phone.CountryCode,
            tenantContact.Phone.NationalNumber
        );
    }
    public static TenantContact ToDomain(this TenantContactDto tenantContactDto)
    {
        return TenantContact.Create(
            Email.Create(tenantContactDto.Email),
            PhoneNumber.Create(tenantContactDto.PhoneCountryCode, tenantContactDto.PhoneNationalNumber)
        );
    }
}
