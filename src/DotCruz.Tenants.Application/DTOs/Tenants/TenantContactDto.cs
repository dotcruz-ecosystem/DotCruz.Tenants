namespace DotCruz.Tenants.Application.DTOs.Tenants;

public sealed record TenantContactDto(
    string Email,
    int PhoneCountryCode,
    string PhoneNationalNumber
);