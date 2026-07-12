using DotCruz.Tenants.Domain.ValueObjects.Communication;

namespace DotCruz.Tenants.Domain.ValueObjects.Tenants;

public record class TenantContact
{
    public Email Email { get; init; } = null!;
    public PhoneNumber Phone { get; init; } = null!;

    private TenantContact() { }

    private TenantContact(Email email, PhoneNumber phone)
    {
        Email = email;
        Phone = phone;
    }

    public static TenantContact Create(Email email, PhoneNumber phone)
    {
        return new TenantContact(email, phone);
    }
}
