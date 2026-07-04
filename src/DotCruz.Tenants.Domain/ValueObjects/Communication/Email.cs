using System.Text.RegularExpressions;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;

namespace DotCruz.Tenants.Domain.ValueObjects.Communication;

public partial record class Email
{
    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled)]
    private static partial Regex EmailRegex();

    public string Value { get; }

    private Email(string address)
    {
        Value = address;
    }

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ErrorOnValidationException(ResourceMessagesException.EMAIL_EMPTY);

        var normalizedAddress = value.Trim().ToLowerInvariant();

        var emailRegex = EmailRegex();

        if (!emailRegex.IsMatch(normalizedAddress))
            throw new ErrorOnValidationException(ResourceMessagesException.EMAIL_INVALID);

        return new Email(normalizedAddress);
    }

    public static implicit operator string(Email email) => email.Value;
}
