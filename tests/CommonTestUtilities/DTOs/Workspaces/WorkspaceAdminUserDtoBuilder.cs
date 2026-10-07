using Bogus;
using DotCruz.Workspaces.Application.DTOs.Workspaces;

namespace CommonTestUtilities.DTOs.Workspaces;

public class WorkspaceAdminUserDtoBuilder
{
    public static WorkspaceAdminUserDto Build(
        bool isAdminNameEmpty = false,
        bool isAdminNameInvalid = false,
        bool isAdminEmailEmpty = false,
        bool isAdminEmailInvalid = false
    )
    {
        var faker = new Faker();

        var name = faker.Person.FullName;
        var email = faker.Person.Email;

        if (isAdminNameEmpty)
            name = string.Empty;

        if (isAdminNameInvalid)
            name = new string('a', 501);

        if (isAdminEmailEmpty)
            email = string.Empty;

        if (isAdminEmailInvalid)
            email = faker.Lorem.Paragraph();

        return new WorkspaceAdminUserDto(name, email);
    }
}
