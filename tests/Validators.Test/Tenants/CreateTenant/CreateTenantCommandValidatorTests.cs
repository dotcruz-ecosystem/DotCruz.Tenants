using CommonTestUtilities.Commands.Tenants;
using DotCruz.Tenants.Application.UseCases.Tenants.CreateTenant;
using DotCruz.Tenants.Domain.Exceptions.Resources;

namespace Validators.Test.Tenants.CreateTenant;

public class CreateTenantCommandValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = CreateTenantCommandBuilder.Build();

        var result = new CreateTenantCommandValidator().Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Error_Name_Empty()
    {
        var request = CreateTenantCommandBuilder.Build(isNameEmpty: true);

        var result = new CreateTenantCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.NAME_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_Slug_Empty()
    {
        var request = CreateTenantCommandBuilder.Build(isSlugEmpty: true);

        var result = new CreateTenantCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.SLUG_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_DocumentNumber_Empty()
    {
        var request = CreateTenantCommandBuilder.Build(isDocumentEmpty: true);

        var result = new CreateTenantCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.DOCUMENT_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_Email_Empty()
    {
        var request = CreateTenantCommandBuilder.Build(isEmailEmpty: true);

        var result = new CreateTenantCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.EMAIL_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_PhoneCountryCode_Invalid()
    {
        var request = CreateTenantCommandBuilder.Build(isCountryCodeInvalid: true);

        var result = new CreateTenantCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.PHONE_COUNTRY_CODE_INVALID, error.ErrorMessage);
    }

    [Fact]
    public void Error_PhoneNationalNumber_Empty()
    {
        var request = CreateTenantCommandBuilder.Build(isPhoneEmpty: true);

        var result = new CreateTenantCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.PHONE_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_Street_Empty()
    {
        var request = CreateTenantCommandBuilder.Build(isStreetEmpty: true);

        var result = new CreateTenantCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.STREET_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_Number_Empty()
    {
        var request = CreateTenantCommandBuilder.Build(isNumberEmpty: true);

        var result = new CreateTenantCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.NUMBER_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_Neighborhood_Empty()
    {
        var request = CreateTenantCommandBuilder.Build(isNeighborhoodEmpty: true);

        var result = new CreateTenantCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.NEIGHBORHOOD_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_City_Empty()
    {
        var request = CreateTenantCommandBuilder.Build(isCityEmpty: true);

        var result = new CreateTenantCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.CITY_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_State_Empty()
    {
        var request = CreateTenantCommandBuilder.Build(isStateEmpty: true);

        var result = new CreateTenantCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.STATE_REQUIRED, error.ErrorMessage);
    }

    [Fact]
    public void Error_ZipCode_Empty()
    {
        var request = CreateTenantCommandBuilder.Build(isZipCodeEmpty: true);

        var result = new CreateTenantCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.ZIPCODE_REQUIRED, error.ErrorMessage);
    }

    [Fact]
    public void Error_TenantDocument_Null()
    {
        var request = CreateTenantCommandBuilder.Build(isTenantDocumentNull: true);

        var result = new CreateTenantCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.DOCUMENT_REQUIRED, error.ErrorMessage);
    }

    [Fact]
    public void Error_TenantContact_Null()
    {
        var request = CreateTenantCommandBuilder.Build(isTenantContactNull: true);

        var result = new CreateTenantCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.CONTACT_REQUIRED, error.ErrorMessage);
    }

    [Fact]
    public void Error_TenantAddress_Null()
    {
        var request = CreateTenantCommandBuilder.Build(isTenantAddressNull: true);

        var result = new CreateTenantCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.ADDRESS_REQUIRED, error.ErrorMessage);
    }

    [Fact]
    public void Error_Tenant_User_Admin_Null()
    {
        var request = CreateTenantCommandBuilder.Build(isTenantAdminUserNull: true);

        var result = new CreateTenantCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.ADMIN_USER_REQUIRED, error.ErrorMessage);
    }

    [Fact]
    public void Error_Tenant_User_Admin_Name_Empty()
    {
        var request = CreateTenantCommandBuilder.Build(isAdminUserNameEmpty: true);

        var result = new CreateTenantCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.NAME_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_Tenant_User_Admin_Name_Invalid()
    {
        var request = CreateTenantCommandBuilder.Build(isAdminUserNameInvalid: true);

        var result = new CreateTenantCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.NAME_INVALID_LENGTH, error.ErrorMessage);
    }

    [Fact]
    public void Error_Tenant_User_Admin_Email_Empty()
    {
        var request = CreateTenantCommandBuilder.Build(isAdminUserEmailEmpty: true);

        var result = new CreateTenantCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceMessagesException.EMAIL_EMPTY);
    }

    [Fact]
    public void Error_Tenant_User_Admin_Email_Invalid()
    {
        var request = CreateTenantCommandBuilder.Build(isAdminUserEmailInvalid: true);

        var result = new CreateTenantCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.EMAIL_INVALID, error.ErrorMessage);
    }
}
