using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;

namespace DotCruz.Tenants.Domain.ValueObjects.Temporal;

public record class DateTimePeriod
{
    public DateTimeOffset Start { get; }
    public DateTimeOffset? End { get; }

    public DateTimePeriod(DateTimeOffset start, DateTimeOffset? end = null)
    {
        if (end.HasValue && end.Value < start)
            throw new ErrorOnValidationException(ResourceMessagesException.SUBSCRIPTION_END_DATE_INVALID);

        Start = start;
        End = end;
    }

    public bool IsExpired => End.HasValue && DateTimeOffset.UtcNow > End.Value;

    public static DateTimePeriod CreateWithDuration(DateTimeOffset start, TimeSpan duration)
    {
        return new DateTimePeriod(start, start.Add(duration));
    }
}
