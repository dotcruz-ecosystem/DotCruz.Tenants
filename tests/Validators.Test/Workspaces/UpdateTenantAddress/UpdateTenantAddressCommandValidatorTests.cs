using CommonTestUtilities.Commands.Tenants;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.UpdateWorkspaceAddress;
using DotCruz.Workspaces.Domain.Exceptions.Resources;

namespace Validators.Test.Tenants.UpdateWorkspaceAddress;

public class UpdateWorkspaceAddressCommandValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = UpdateWorkspaceAddressCommandBuilder.Build();

        var result = new UpdateWorkspaceAddressCommandValidator().Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Error_Id_Empty()
    {
        var request = UpdateWorkspaceAddressCommandBuilder.Build(id: Guid.Empty);

        var result = new UpdateWorkspaceAddressCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.ID_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_Street_Empty()
    {
        var request = UpdateWorkspaceAddressCommandBuilder.Build(isStreetEmpty: true);

        var result = new UpdateWorkspaceAddressCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.STREET_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_Number_Empty()
    {
        var request = UpdateWorkspaceAddressCommandBuilder.Build(isNumberEmpty: true);

        var result = new UpdateWorkspaceAddressCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.NUMBER_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_Neighborhood_Empty()
    {
        var request = UpdateWorkspaceAddressCommandBuilder.Build(isNeighborhoodEmpty: true);

        var result = new UpdateWorkspaceAddressCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.NEIGHBORHOOD_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_City_Empty()
    {
        var request = UpdateWorkspaceAddressCommandBuilder.Build(isCityEmpty: true);

        var result = new UpdateWorkspaceAddressCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.CITY_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_State_Empty()
    {
        var request = UpdateWorkspaceAddressCommandBuilder.Build(isStateEmpty: true);

        var result = new UpdateWorkspaceAddressCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.STATE_REQUIRED, error.ErrorMessage);
    }

    [Fact]
    public void Error_ZipCode_Empty()
    {
        var request = UpdateWorkspaceAddressCommandBuilder.Build(isZipCodeEmpty: true);

        var result = new UpdateWorkspaceAddressCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.ZIPCODE_REQUIRED, error.ErrorMessage);
    }

    [Fact]
    public void Error_WorkspaceAddress_Null()
    {
        var request = UpdateWorkspaceAddressCommandBuilder.Build(isWorkspaceAddressNull: true);

        var result = new UpdateWorkspaceAddressCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.ADDRESS_REQUIRED, error.ErrorMessage);
    }
}
