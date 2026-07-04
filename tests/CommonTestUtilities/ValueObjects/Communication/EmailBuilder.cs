using Bogus;
using DotCruz.Tenants.Domain.ValueObjects.Communication;

namespace CommonTestUtilities.ValueObjects.Communication;

public class EmailBuilder
{
    public static Email Build(bool isEmpty = false, bool isInvalid = false)
    {
        var faker = new Faker();

        var email = faker.Internet.Email();

        if (isEmpty)
            email = string.Empty;

        if (isInvalid)
            email = faker.Person.FullName;

        return Email.Create(email);
    }
}
