using Bogus;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.CreateWorkspace;
using DotCruz.Tenants.Domain.Enums.Tenants;

namespace CommonTestUtilities.Commands.Tenants;

public class CreateWorkspaceCommandBuilder
{
    public static CreateWorkspaceCommand Build(
        bool isNameEmpty = false,
        bool isNameInvalid = false,
        bool isSlugEmpty = false,
        bool isSlugInvalid = false,
        bool isSlugReserved = false,
        TenantType? type = null,
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
                    Type: type ?? f.PickRandom<TenantType>(),
                    OwnerEmail: email
                );
            });

        return faker.Generate();
    }
}
