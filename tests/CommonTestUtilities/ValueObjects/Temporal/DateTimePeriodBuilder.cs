using Bogus;
using DotCruz.Tenants.Domain.ValueObjects.Temporal;

namespace CommonTestUtilities.ValueObjects.Temporal;

public class DateTimePeriodBuilder
{
    public static DateTimePeriod Build(bool isEndDateInvalid = false)
    {
        var faker = new Faker();

        var startDate = faker.Date.Recent();
        var endDate = isEndDateInvalid ? faker.Date.Past() : faker.Date.Soon();

        return new DateTimePeriod(startDate, endDate);
    }

    public static DateTimePeriod BuildWithDuration()
    {
        var faker = new Faker();
        
        var startDate = faker.Date.Recent();
        var duration = faker.Date.Timespan();

        return DateTimePeriod.CreateWithDuration(startDate, duration);
    }
}
