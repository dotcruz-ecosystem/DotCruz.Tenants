using CommonTestUtilities.ValueObjects.Communication;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using DotCruz.Tenants.Domain.ValueObjects.Communication;

namespace Domain.Test.ValueObjects.Communication;

public class PhoneNumberTests
{
    [Fact]
    public void Success()
    {
        var phoneNumber = PhoneNumberBuilder.Build();

        Assert.NotNull(phoneNumber);

        var countryCode = phoneNumber.CountryCode;
        var nationalNumber = phoneNumber.NationalNumber;

        Assert.InRange(countryCode, 1, 999);
        Assert.NotEmpty(nationalNumber);
        Assert.Equal($"+{countryCode}{nationalNumber}", phoneNumber.GetFullNumber());
    }

    [Fact]
    public void Error_Country_Code_Invalid()
    {
        static PhoneNumber act() => PhoneNumberBuilder.Build(isCountryCodeInvalid: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);

        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());

        Assert.Equal(ResourceMessagesException.PHONE_COUNTRY_CODE_INVALID, exceptionMessage);
    }

    [Fact]
    public void Error_Phone_Empty()
    {
        static PhoneNumber act() => PhoneNumberBuilder.Build(isPhoneEmpty: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);

        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());

        Assert.Equal(ResourceMessagesException.PHONE_EMPTY, exceptionMessage);
    }

    [Fact]
    public void Error_Phone_Invalid()
    {
        static PhoneNumber act() => PhoneNumberBuilder.Build(isPhoneInvalid: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);

        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());

        Assert.Equal(ResourceMessagesException.PHONE_INVALID, exceptionMessage);
    }
}
