using CommonTestUtilities.ValueObjects.Location;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;
using DotCruz.Workspaces.Domain.ValueObjects.Location;

namespace Domain.Test.ValueObjects.Location;

public class ZipCodeTests
{
    [Fact]
    public void Success()
    {
        var zipCode = ZipCodeBuilder.Build();

        Assert.NotNull(zipCode);
        Assert.NotEmpty(zipCode.Value);
    }

    [Fact]
    public void Error_Invalid_Zip_Code()
    {
        static ZipCode act() => ZipCodeBuilder.Build(isEmpty: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.ZIPCODE_INVALID, exceptionMessage);
    }
}
