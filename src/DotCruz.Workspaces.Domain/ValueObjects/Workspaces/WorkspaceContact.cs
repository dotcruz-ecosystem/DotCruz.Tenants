using DotCruz.Workspaces.Domain.ValueObjects.Communication;

namespace DotCruz.Workspaces.Domain.ValueObjects.Workspaces;

public record class WorkspaceContact
{
    public Email Email { get; init; } = null!;
    public PhoneNumber Phone { get; init; } = null!;

    private WorkspaceContact() { }

    private WorkspaceContact(Email email, PhoneNumber phone)
    {
        Email = email;
        Phone = phone;
    }

    public static WorkspaceContact Create(Email email, PhoneNumber phone)
    {
        return new WorkspaceContact(email, phone);
    }
}
