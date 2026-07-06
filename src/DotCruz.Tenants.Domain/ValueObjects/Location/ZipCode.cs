using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using System.Text.RegularExpressions;

namespace DotCruz.Tenants.Domain.ValueObjects.Location;

public partial record class ZipCode
{
    public string Value { get; }

    private ZipCode(string value)
    {
        Value = value;
    }

    public static ZipCode Create(string value)
    {
        var cleanZip = OnlyDigitsRegex().Replace(value ?? string.Empty, "");
        
        if (cleanZip.Length != 8)
            throw new ErrorOnValidationException(ResourceMessagesException.ZIPCODE_INVALID);

        return new ZipCode(cleanZip);
    }

    public string FormattedValue => $"{Value[..5]}-{Value[5..]}";

    public override string ToString() => FormattedValue;

    public static implicit operator string(ZipCode zipCode) => zipCode.Value;

    [GeneratedRegex(@"[^\d]")]
    private static partial Regex OnlyDigitsRegex();
}
