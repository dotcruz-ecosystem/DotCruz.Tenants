using Bogus;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.AddTenantMember;
using DotCruz.Tenants.Domain.Enums.Tenants;

namespace CommonTestUtilities.Commands.Tenants;

public class AddTenantMemberCommandBuilder
{
    public static AddTenantMemberCommand Build(
        Guid? tenantId = null,
        string? email = null,
        TenantRole role = TenantRole.Member,
        Guid? userId = null,
        bool isTenantIdEmpty = false,
        bool isEmailEmpty = false,
        bool isEmailInvalid = false)
    {
        var faker = new Faker();
        var emailValue = email ?? faker.Internet.Email();

        if (isEmailEmpty)
            emailValue = string.Empty;
        else if (isEmailInvalid)
            emailValue = "not-an-email";

        return new AddTenantMemberCommand(
            isTenantIdEmpty ? Guid.Empty : (tenantId ?? Guid.NewGuid()),
            emailValue,
            role,
            userId
        );
    }
}
