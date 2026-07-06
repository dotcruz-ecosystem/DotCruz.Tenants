using CommonTestUtilities.ValueObjects.Identity;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using DotCruz.Tenants.Domain.ValueObjects.Identity;

namespace Domain.Test.ValueObjects.Identity;

public class NameTests
{
    [Fact]
    public void Success()
    {
        var name = NameBuilder.Build();

        Assert.NotNull(name);
        Assert.NotEmpty(name.Value);
        Assert.InRange(name.Value.Length, 2, 100);
    }

    [Fact]
    public void Error_Name_Empty()
    {
        static Name act() => NameBuilder.Build(isEmpty: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.NAME_EMPTY, exceptionMessage);
    }

    [Fact]
    public void Error_Name_Invalid_Length()
    {
        static Name act() => NameBuilder.Build(isInvalidLength: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.NAME_INVALID_LENGTH, exceptionMessage);
    }
}
