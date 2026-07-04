using CommonTestUtilities.ValueObjects.Communication;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using DotCruz.Tenants.Domain.ValueObjects.Communication;

namespace Domain.Test.ValueObjects.Communication;

public class EmailTests
{
    [Fact]
    public void Success()
    {
        var email = EmailBuilder.Build();

        Assert.NotNull(email);
        Assert.NotEmpty(email.Value);
    }

    [Fact]
    public void Error_Email_Empty()
    {
        static Email act() => EmailBuilder.Build(isEmpty: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);

        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());

        Assert.Equal(ResourceMessagesException.EMAIL_EMPTY, exceptionMessage);
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        static Email act() => EmailBuilder.Build(isInvalid: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);

        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());

        Assert.Equal(ResourceMessagesException.EMAIL_INVALID, exceptionMessage);
    }
}
