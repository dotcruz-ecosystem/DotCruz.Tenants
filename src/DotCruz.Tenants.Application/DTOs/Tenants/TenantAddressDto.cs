namespace DotCruz.Tenants.Application.DTOs.Tenants;

public sealed record TenantAddressDto(
    string Street,
    string Number,
    string? Complement,
    string Neighborhood,
    string City,
    string State,
    string ZipCode
);
