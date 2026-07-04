using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using System.Text.RegularExpressions;

namespace DotCruz.Tenants.Domain.ValueObjects.Location;

public record class ZipCode
{
    public string Value { get; }

    public ZipCode(string value)
    {
        var cleanZip = Regex.Replace(value ?? string.Empty, @"[^\d]", "");
        if (cleanZip.Length != 8)
            throw new ErrorOnValidationException(ResourceMessagesException.ZIPCODE_INVALID);

        Value = cleanZip;
    }

    public string FormattedValue => $"{Value[..5]}-{Value[5..]}";

    public override string ToString() => FormattedValue;

    public static implicit operator string(ZipCode zipCode) => zipCode.Value;
}
