using Bogus;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.AddWorkspaceMember;
using DotCruz.Workspaces.Domain.Enums.Workspaces;

namespace CommonTestUtilities.Commands.Tenants;

public class AddWorkspaceMemberCommandBuilder
{
    public static AddWorkspaceMemberCommand Build(
        Guid? WorkspaceId = null,
        string? email = null,
        WorkspaceRole role = WorkspaceRole.Member,
        Guid? userId = null,
        bool isWorkspaceIdEmpty = false,
        bool isEmailEmpty = false,
        bool isEmailInvalid = false)
    {
        var faker = new Faker();
        var emailValue = email ?? faker.Internet.Email();

        if (isEmailEmpty)
            emailValue = string.Empty;
        else if (isEmailInvalid)
            emailValue = "not-an-email";

        return new AddWorkspaceMemberCommand(
            isWorkspaceIdEmpty ? Guid.Empty : (WorkspaceId ?? Guid.NewGuid()),
            emailValue,
            role,
            userId
        );
    }
}
