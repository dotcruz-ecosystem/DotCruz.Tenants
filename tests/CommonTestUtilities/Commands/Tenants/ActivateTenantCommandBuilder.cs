using Bogus;
using DotCruz.Tenants.Application.UseCases.Tenants.ActivateTenant;

namespace CommonTestUtilities.Commands.Tenants;

public class ActivateTenantCommandBuilder
{
    public static ActivateTenantCommand Build(Guid? id = null)
    {
        var faker = new Faker<ActivateTenantCommand>()
            .CustomInstantiator(f => new ActivateTenantCommand(
                Id: id ?? Guid.NewGuid()
            ));

        return faker.Generate();
    }
}
