using System.Text.RegularExpressions;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;

namespace DotCruz.Tenants.Domain.ValueObjects.Communication;

public partial record class PhoneNumber
{
    [GeneratedRegex(@"[^\d]", RegexOptions.Compiled)]
    private static partial Regex OnlyDigitsRegex();

    public int CountryCode { get; }
    public string NationalNumber { get; }

    private PhoneNumber(int countryCode, string nationalNumber) 
    {
        CountryCode = countryCode;
        NationalNumber = nationalNumber;
    }

    public static PhoneNumber Create(int countryCode, string nationalNumber)
    {
        if (countryCode <= 0 || countryCode > 999)
            throw new ErrorOnValidationException(ResourceMessagesException.PHONE_COUNTRY_CODE_INVALID);

        if (string.IsNullOrWhiteSpace(nationalNumber))
            throw new ErrorOnValidationException(ResourceMessagesException.PHONE_EMPTY);

        var cleanNumber = OnlyDigitsRegex().Replace(nationalNumber, "");

        if (cleanNumber.Length < 4 || cleanNumber.Length > 14)
            throw new ErrorOnValidationException(ResourceMessagesException.PHONE_INVALID);

        return new PhoneNumber(countryCode, cleanNumber);
    }

    public string GetFullNumber() => $"+{CountryCode}{NationalNumber}";

    public static implicit operator string(PhoneNumber phoneNumber) => phoneNumber.GetFullNumber();
}
