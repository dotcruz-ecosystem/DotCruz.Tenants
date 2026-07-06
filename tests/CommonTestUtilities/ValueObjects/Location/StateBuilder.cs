using DotCruz.Tenants.Domain.ValueObjects.Location;

namespace CommonTestUtilities.ValueObjects.Location;

public class StateBuilder
{
    public static State Build(bool isEmpty = false, bool isInvalidLength = false)
    {
        var faker = new Bogus.Faker();
        var state = faker.Address.StateAbbr();
        
        if (isEmpty)
            state = string.Empty;
        
        if (isInvalidLength)
            state = faker.Random.String2(faker.Random.Number(3, 10));
        
        return State.Create(state);
    }
}
