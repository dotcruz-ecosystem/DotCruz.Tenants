using Bogus;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.DeactivateTenant;

namespace CommonTestUtilities.Commands.Tenants;

public class DeactivateTenantCommandBuilder
{
    public static DeactivateTenantCommand Build(Guid? id = null)
    {
        var faker = new Faker<DeactivateTenantCommand>()
            .CustomInstantiator(f => new DeactivateTenantCommand(
                Id: id ?? Guid.NewGuid()
            ));

        return faker.Generate();
    }
}
