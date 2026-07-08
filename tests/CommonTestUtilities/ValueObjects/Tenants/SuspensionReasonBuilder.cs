using Bogus;
using DotCruz.Tenants.Domain.ValueObjects.Tenants;

namespace CommonTestUtilities.ValueObjects.Tenants;

public class SuspensionReasonBuilder
{
    public static SuspensionReason Build(
        bool isEmpty = false, 
        bool isTooShort = false, 
        bool isTooLong = false)
    {
        var faker = new Faker();

        var value = faker.Lorem.Paragraph();

        if (isEmpty)
            value = string.Empty;

        if (isTooShort)
            value = faker.Random.String2(faker.Random.Number(1, 4));

        if (isTooLong)
            value = faker.Random.String2(faker.Random.Number(501, 1000));

        return SuspensionReason.Create(value);
    }
}
