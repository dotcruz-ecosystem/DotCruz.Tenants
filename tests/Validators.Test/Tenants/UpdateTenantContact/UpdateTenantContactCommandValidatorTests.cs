using CommonTestUtilities.Commands.Tenants;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.UpdateTenantContact;
using DotCruz.Tenants.Domain.Exceptions.Resources;

namespace Validators.Test.Tenants.UpdateTenantContact;

public class UpdateTenantContactCommandValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = UpdateTenantContactCommandBuilder.Build();

        var result = new UpdateTenantContactCommandValidator().Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Error_Id_Empty()
    {
        var request = UpdateTenantContactCommandBuilder.Build(id: Guid.Empty);

        var result = new UpdateTenantContactCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.ID_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_Email_Empty()
    {
        var request = UpdateTenantContactCommandBuilder.Build(isEmailEmpty: true);

        var result = new UpdateTenantContactCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.EMAIL_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_PhoneCountryCode_Invalid()
    {
        var request = UpdateTenantContactCommandBuilder.Build(isCountryCodeInvalid: true);

        var result = new UpdateTenantContactCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.PHONE_COUNTRY_CODE_INVALID, error.ErrorMessage);
    }

    [Fact]
    public void Error_PhoneNationalNumber_Empty()
    {
        var request = UpdateTenantContactCommandBuilder.Build(isPhoneEmpty: true);

        var result = new UpdateTenantContactCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.PHONE_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_TenantContact_Null()
    {
        var request = UpdateTenantContactCommandBuilder.Build(isTenantContactNull: true);

        var result = new UpdateTenantContactCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.CONTACT_REQUIRED, error.ErrorMessage);
    }
}
