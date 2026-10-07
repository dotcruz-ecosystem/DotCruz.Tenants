using Bogus;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.ProvisionPersonalWorkspace;

namespace CommonTestUtilities.Commands.Tenants;

public class ProvisionPersonalWorkspaceCommandBuilder
{
    public static ProvisionPersonalWorkspaceCommand Build(
        Guid? userId = null,
        string? name = null,
        string? email = null)
    {
        var faker = new Faker<ProvisionPersonalWorkspaceCommand>()
            .CustomInstantiator(f =>
            {
                return new ProvisionPersonalWorkspaceCommand(
                    UserId: userId ?? Guid.NewGuid(),
                    Name: name ?? f.Person.FullName,
                    Email: email ?? f.Internet.Email()
                );
            });

        return faker.Generate();
    }
}
