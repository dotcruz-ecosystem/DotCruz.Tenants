using System.Text.RegularExpressions;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;

namespace DotCruz.Tenants.Domain.ValueObjects.Identity;

public partial record class Name
{
    [GeneratedRegex(@"\s+")]
    private static partial Regex MultipleSpacesRegex();

    public string Value { get; }

    private Name(string value)
    {
        Value = value;
    }

    public static Name Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ErrorOnValidationException(ResourceMessagesException.NAME_EMPTY);

        var trimmedName = value.Trim();

        var cleanName = MultipleSpacesRegex().Replace(trimmedName, " ");

        if (cleanName.Length < 2 || cleanName.Length > 100)
            throw new ErrorOnValidationException(ResourceMessagesException.NAME_INVALID_LENGTH);

        return new Name(cleanName);
    }

    public static implicit operator string(Name name) => name.Value;
}
