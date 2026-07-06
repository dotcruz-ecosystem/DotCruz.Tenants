using Bogus;
using DotCruz.Tenants.Domain.ValueObjects.Tenants;

namespace CommonTestUtilities.ValueObjects.Tenants;

public class ResourceLimitsBuilder
{
    public static ResourceLimits Build(
        bool hasInvalidMaxUsers = false, 
        bool hasInvalidMaxEmails = false
    )
    {
        var faker = new Faker();

        var maxUsers = faker.Random.Int(1, 100);
        var maxEmails = faker.Random.Int(100, 100000);

        if (hasInvalidMaxUsers)
            maxUsers = faker.Random.Int(-100, 0);

        if (hasInvalidMaxEmails)
            maxEmails = faker.Random.Int(-1000, 0);

        return new ResourceLimits(maxUsers, maxEmails);
    }
}
