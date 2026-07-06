using CommonTestUtilities.ValueObjects.Communication;
using DotCruz.Tenants.Domain.ValueObjects.Tenants;

namespace CommonTestUtilities.ValueObjects.Tenants;

public class TenantContactBuilder
{
    public static TenantContact Build()
    {
        var email = EmailBuilder.Build();
        var phone = PhoneNumberBuilder.Build();

        return new TenantContact(email, phone);
    }
}
