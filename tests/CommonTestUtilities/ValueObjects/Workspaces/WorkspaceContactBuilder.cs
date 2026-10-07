using CommonTestUtilities.ValueObjects.Communication;
using DotCruz.Workspaces.Domain.ValueObjects.Workspaces;

namespace CommonTestUtilities.ValueObjects.Workspaces;

public class WorkspaceContactBuilder
{
    public static WorkspaceContact Build()
    {
        var email = EmailBuilder.Build();
        var phone = PhoneNumberBuilder.Build();

        return WorkspaceContact.Create(email, phone);
    }
}
