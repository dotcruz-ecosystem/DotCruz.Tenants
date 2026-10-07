using Bogus;
using CommonTestUtilities.DTOs.Workspaces;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.UpdateWorkspaceContact;

namespace CommonTestUtilities.Commands.Tenants;

public class UpdateWorkspaceContactCommandBuilder
{
    public static UpdateWorkspaceContactCommand Build(
        Guid? id = null,
        bool isWorkspaceContactNull = false,
        bool isEmailEmpty = false,
        bool isCountryCodeInvalid = false,
        bool isPhoneEmpty = false
    )
    {
        var faker = new Faker<UpdateWorkspaceContactCommand>()
            .CustomInstantiator(f => new UpdateWorkspaceContactCommand(
                Id: id ?? Guid.NewGuid(),
                WorkspaceContact: isWorkspaceContactNull ? null! : WorkspaceContactDtoBuilder.Build(
                    isEmailEmpty: isEmailEmpty,
                    isCountryCodeInvalid: isCountryCodeInvalid,
                    isPhoneEmpty: isPhoneEmpty
                )
            ));

        return faker.Generate();
    }
}
