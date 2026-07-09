using Bogus;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.SuspendTenant;

namespace CommonTestUtilities.Commands.Tenants;

public class SuspendTenantCommandBuilder
{
    public static SuspendTenantCommand Build(
        Guid? id = null,
        bool isReasonEmpty = false,
        bool isReasonTooShort = false,
        bool isReasonTooLong = false
    )
    {
        var faker = new Faker("pt_BR");
        
        string reason;
        if (isReasonEmpty)
            reason = string.Empty;
        else if (isReasonTooShort)
            reason = "abc";
        else if (isReasonTooLong)
            reason = new string('a', 501); 
        else
        {
            var paragraph = faker.Lorem.Paragraph();
            reason = paragraph.Length > 500 ? paragraph[..500] : paragraph;
        }

        return new SuspendTenantCommand(
            Id: id ?? Guid.NewGuid(),
            Reason: reason
        );
    }
}
