using DotCruz.Tenants.Domain.ValueObjects.Communication;

namespace DotCruz.Tenants.Domain.ValueObjects.Tenants;

public record class TenantContact
{
    public Email Email { get; }
    public PhoneNumber Phone { get; }

    private TenantContact(Email email, PhoneNumber phoneNumber)
    {
        Email = email;
        Phone = phoneNumber;
    }

    public static TenantContact Create(Email email, PhoneNumber phoneNumber)
    {
        return new TenantContact(email, phoneNumber);
    }
}
