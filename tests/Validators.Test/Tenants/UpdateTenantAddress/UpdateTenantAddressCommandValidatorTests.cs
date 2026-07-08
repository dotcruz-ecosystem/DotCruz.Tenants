using CommonTestUtilities.Commands.Tenants;
using DotCruz.Tenants.Application.UseCases.Tenants.UpdateTenantAddress;
using DotCruz.Tenants.Domain.Exceptions.Resources;

namespace Validators.Test.Tenants.UpdateTenantAddress;

public class UpdateTenantAddressCommandValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = UpdateTenantAddressCommandBuilder.Build();

        var result = new UpdateTenantAddressCommandValidator().Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Error_Id_Empty()
    {
        var request = UpdateTenantAddressCommandBuilder.Build(id: Guid.Empty);

        var result = new UpdateTenantAddressCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.ID_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_Street_Empty()
    {
        var request = UpdateTenantAddressCommandBuilder.Build(isStreetEmpty: true);

        var result = new UpdateTenantAddressCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.STREET_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_Number_Empty()
    {
        var request = UpdateTenantAddressCommandBuilder.Build(isNumberEmpty: true);

        var result = new UpdateTenantAddressCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.NUMBER_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_Neighborhood_Empty()
    {
        var request = UpdateTenantAddressCommandBuilder.Build(isNeighborhoodEmpty: true);

        var result = new UpdateTenantAddressCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.NEIGHBORHOOD_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_City_Empty()
    {
        var request = UpdateTenantAddressCommandBuilder.Build(isCityEmpty: true);

        var result = new UpdateTenantAddressCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.CITY_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_State_Empty()
    {
        var request = UpdateTenantAddressCommandBuilder.Build(isStateEmpty: true);

        var result = new UpdateTenantAddressCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.STATE_REQUIRED, error.ErrorMessage);
    }

    [Fact]
    public void Error_ZipCode_Empty()
    {
        var request = UpdateTenantAddressCommandBuilder.Build(isZipCodeEmpty: true);

        var result = new UpdateTenantAddressCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.ZIPCODE_REQUIRED, error.ErrorMessage);
    }

    [Fact]
    public void Error_TenantAddress_Null()
    {
        var request = UpdateTenantAddressCommandBuilder.Build(isTenantAddressNull: true);

        var result = new UpdateTenantAddressCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.ADDRESS_REQUIRED, error.ErrorMessage);
    }
}
