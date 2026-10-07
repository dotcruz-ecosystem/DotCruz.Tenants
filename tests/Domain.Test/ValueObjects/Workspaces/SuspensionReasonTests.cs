using CommonTestUtilities.ValueObjects.Workspaces;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;
using DotCruz.Workspaces.Domain.ValueObjects.Workspaces;

namespace Domain.Test.ValueObjects.Workspaces;

public class SuspensionReasonTests
{
    [Fact]
    public void Success()
    {
        var reason = SuspensionReasonBuilder.Build();

        Assert.NotNull(reason);
        Assert.NotEmpty(reason.Value);
        Assert.True(reason.Value.Length >= 5 && reason.Value.Length <= 500);
        Assert.Equal(reason.Value, reason.ToString());
        Assert.Equal(reason.Value, (string)reason);
    }

    [Fact]
    public void Error_SuspensionReason_Empty()
    {
        static SuspensionReason act() => SuspensionReasonBuilder.Build(isEmpty: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.SUSPENSION_REASON_EMPTY, exceptionMessage);
    }

    [Fact]
    public void Error_SuspensionReason_TooShort()
    {
        static SuspensionReason act() => SuspensionReasonBuilder.Build(isTooShort: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.SUSPENSION_REASON_INVALID, exceptionMessage);
    }

    [Fact]
    public void Error_SuspensionReason_TooLong()
    {
        static SuspensionReason act() => SuspensionReasonBuilder.Build(isTooLong: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.SUSPENSION_REASON_INVALID, exceptionMessage);
    }
}
