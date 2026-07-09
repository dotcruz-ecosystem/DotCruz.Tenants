using Bogus;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.TerminateTenant;

namespace CommonTestUtilities.Commands.Tenants;

public class TerminateTenantCommandBuilder
{
    public static TerminateTenantCommand Build(Guid? id = null)
    {
        var faker = new Faker<TerminateTenantCommand>()
            .CustomInstantiator(f => new TerminateTenantCommand(
                Id: id ?? Guid.NewGuid()
            ));

        return faker.Generate();
    }
}
