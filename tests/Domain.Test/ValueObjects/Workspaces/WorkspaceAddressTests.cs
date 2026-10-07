using CommonTestUtilities.ValueObjects.Workspaces;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;
using DotCruz.Workspaces.Domain.ValueObjects.Workspaces;

namespace Domain.Test.ValueObjects.Workspaces;

public class WorkspaceAddressTests
{
    [Fact]
    public void Success()
    {
        var address = WorkspaceAddressBuilder.Build();

        Assert.NotNull(address);
        Assert.NotEmpty(address.Street);
        Assert.NotEmpty(address.Number);
        Assert.NotEmpty(address.Neighborhood);
        Assert.NotEmpty(address.City);
        Assert.NotNull(address.State);
        Assert.NotNull(address.ZipCode);
    }

    [Fact]
    public void Success_ToString_With_Complement()
    {
        var address = WorkspaceAddressBuilder.Build();
        var expected = $"{address.Street}, {address.Number}, {address.Complement} - {address.Neighborhood}, {address.City} - {address.State}, CEP {address.ZipCode.FormattedValue}";

        Assert.Equal(expected, address.ToString());
    }

    [Fact]
    public void Success_ToString_Without_Complement()
    {
        var baseAddress = WorkspaceAddressBuilder.Build();
        var address = WorkspaceAddress.Create(
            baseAddress.Street,
            baseAddress.Number,
            complement: null,
            baseAddress.Neighborhood,
            baseAddress.City,
            baseAddress.State,
            baseAddress.ZipCode
        );

        var expected = $"{address.Street}, {address.Number} - {address.Neighborhood}, {address.City} - {address.State}, CEP {address.ZipCode.FormattedValue}";

        Assert.Equal(expected, address.ToString());
    }

    [Fact]
    public void Error_Street_Empty()
    {
        static WorkspaceAddress act() => WorkspaceAddressBuilder.Build(isStreetEmpty: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.STREET_EMPTY, exceptionMessage);
    }

    [Fact]
    public void Error_Number_Empty()
    {
        static WorkspaceAddress act() => WorkspaceAddressBuilder.Build(isNumberEmpty: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.NUMBER_EMPTY, exceptionMessage);
    }

    [Fact]
    public void Error_Neighborhood_Empty()
    {
        static WorkspaceAddress act() => WorkspaceAddressBuilder.Build(isNeighborhoodEmpty: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.NEIGHBORHOOD_EMPTY, exceptionMessage);
    }

    [Fact]
    public void Error_City_Empty()
    {
        static WorkspaceAddress act() => WorkspaceAddressBuilder.Build(isCityEmpty: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.CITY_EMPTY, exceptionMessage);
    }

    [Fact]
    public void Error_State_Required()
    {
        static WorkspaceAddress act() => WorkspaceAddressBuilder.Build(isStateNull: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.STATE_REQUIRED, exceptionMessage);
    }

    [Fact]
    public void Error_ZipCode_Required()
    {
        static WorkspaceAddress act() => WorkspaceAddressBuilder.Build(isZipCodeNull: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.ZIPCODE_REQUIRED, exceptionMessage);
    }
}
