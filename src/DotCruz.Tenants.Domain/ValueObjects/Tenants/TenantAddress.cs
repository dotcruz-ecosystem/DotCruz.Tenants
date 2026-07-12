using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using DotCruz.Tenants.Domain.ValueObjects.Location;

namespace DotCruz.Tenants.Domain.ValueObjects.Tenants;

public record class TenantAddress
{
    public string Street { get; init; } = null!;
    public string Number { get; init; } = null!;
    public string? Complement { get; init; }
    public string Neighborhood { get; init; } = null!;
    public string City { get; init; } = null!;
    public State State { get; init; } = null!;
    public ZipCode ZipCode { get; init; } = null!;

    private TenantAddress() { }

    private TenantAddress(
        string street,
        string number,
        string? complement,
        string neighborhood,
        string city,
        State state,
        ZipCode zipCode) 
    {
        Street = street.Trim();
        Number = number.Trim();
        Complement = complement?.Trim();
        Neighborhood = neighborhood.Trim();
        City = city.Trim();
        State = state;
        ZipCode = zipCode;
    }

    public static TenantAddress Create(
        string street, 
        string number, 
        string? complement, 
        string neighborhood, 
        string city, 
        State state, 
        ZipCode zipCode)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ErrorOnValidationException(ResourceMessagesException.STREET_EMPTY);

        if (string.IsNullOrWhiteSpace(number))
            throw new ErrorOnValidationException(ResourceMessagesException.NUMBER_EMPTY);

        if (string.IsNullOrWhiteSpace(neighborhood))
            throw new ErrorOnValidationException(ResourceMessagesException.NEIGHBORHOOD_EMPTY);

        if (string.IsNullOrWhiteSpace(city))
            throw new ErrorOnValidationException(ResourceMessagesException.CITY_EMPTY);

        if (state == null)
            throw new ErrorOnValidationException(ResourceMessagesException.STATE_REQUIRED);

        if (zipCode == null)
            throw new ErrorOnValidationException(ResourceMessagesException.ZIPCODE_REQUIRED);

        return new TenantAddress(street, number, complement, neighborhood, city, state, zipCode);
    }

    public override string ToString()
    {
        var comp = !string.IsNullOrWhiteSpace(Complement) ? $", {Complement}" : "";
        return $"{Street}, {Number}{comp} - {Neighborhood}, {City} - {State}, CEP {ZipCode.FormattedValue}";
    }
}
