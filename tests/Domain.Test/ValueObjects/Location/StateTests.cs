using CommonTestUtilities.ValueObjects.Location;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using DotCruz.Tenants.Domain.ValueObjects.Location;

namespace Domain.Test.ValueObjects.Location;

public class StateTests
{
    [Fact]
    public void Success()
    {
        var state = StateBuilder.Build();

        Assert.NotNull(state);
        Assert.NotEmpty(state.Value);
        Assert.Equal(2, state.Value.Length);
    }

    [Fact]
    public void Error_State_Empty()
    {
        static State act() => StateBuilder.Build(isEmpty: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.STATE_INVALID, exceptionMessage);
    }

    [Fact]
    public void Error_State_Invalid_Length()
    {
        static State act() => StateBuilder.Build(isInvalidLength: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.STATE_INVALID, exceptionMessage);
    }
}
