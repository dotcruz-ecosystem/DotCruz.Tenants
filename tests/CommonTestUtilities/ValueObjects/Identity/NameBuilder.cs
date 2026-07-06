using Bogus;
using DotCruz.Tenants.Domain.ValueObjects.Identity;

namespace CommonTestUtilities.ValueObjects.Identity;

public class NameBuilder
{
    public static Name Build(bool isEmpty = false, bool isInvalidLength = false)
    {
        var faker = new Faker();

        var name = faker.Person.FullName;

        if (isEmpty)
            name = string.Empty;
        
        if (isInvalidLength)
            name = faker.Random.String2(faker.Random.Number(101, 999));

        return Name.Create(name);
    }
}
