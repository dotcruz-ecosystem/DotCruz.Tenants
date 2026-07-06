using CommonTestUtilities.ValueObjects.Temporal;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using DotCruz.Tenants.Domain.ValueObjects.Temporal;

namespace Domain.Test.ValueObjects.Temporal;

public class DateTimePeriodTests
{
    [Fact]
    public void Success()
    {
        var dateTimePeriod = DateTimePeriodBuilder.Build();
        
        Assert.NotNull(dateTimePeriod);
        Assert.True(dateTimePeriod.Start < dateTimePeriod.End);
    }

    [Fact]
    public void Success_With_Duration()
    {
        var dateTimePeriod = DateTimePeriodBuilder.BuildWithDuration();

        Assert.NotNull(dateTimePeriod);
        Assert.True(dateTimePeriod.Start < dateTimePeriod.End);
    }

    [Fact]
    public void Error_Invalid_End_Date()
    {
        static DateTimePeriod act() => DateTimePeriodBuilder.Build(isEndDateInvalid: true);
        
        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.SUBSCRIPTION_END_DATE_INVALID, exceptionMessage);
    }
}
