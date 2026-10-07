using Bogus;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.CreateWorkspace;
using DotCruz.Workspaces.Domain.Enums.Workspaces;

namespace CommonTestUtilities.Commands.Tenants;

public class CreateWorkspaceCommandBuilder
{
    public static CreateWorkspaceCommand Build(
        bool isNameEmpty = false,
        bool isNameInvalid = false,
        bool isSlugEmpty = false,
        bool isSlugInvalid = false,
        bool isSlugReserved = false,
        WorkspaceType? type = null,
        string? ownerEmail = null,
        bool isOwnerEmailInvalid = false
    )
    {
        var faker = new Faker<CreateWorkspaceCommand>()
            .CustomInstantiator(f =>
            {
                var name = isNameEmpty
                    ? string.Empty
                    : isNameInvalid
                        ? "a"
                        : f.Company.CompanyName();

                var slug = isSlugEmpty
                    ? string.Empty
                    : isSlugInvalid
                        ? "ab"
                        : isSlugReserved
                            ? "admin"
                            : (f.Internet.DomainWord() + "xyz").ToLowerInvariant();

                var email = isOwnerEmailInvalid
                    ? "invalid-email"
                    : ownerEmail;

                return new CreateWorkspaceCommand(
                    Name: name,
                    Slug: slug,
                    Type: type ?? f.PickRandom<WorkspaceType>(),
                    OwnerEmail: email
                );
            });

        return faker.Generate();
    }
}
