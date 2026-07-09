using Bogus;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.UpdateTenantSubscription;
using DotCruz.Tenants.Domain.Enums.Tenants;

namespace CommonTestUtilities.Commands.Tenants;

public class UpdateTenantSubscriptionCommandBuilder
{
    public static UpdateTenantSubscriptionCommand Build(
        Guid? id = null,
        PlanType? plan = null
    )
    {
        var faker = new Faker<UpdateTenantSubscriptionCommand>()
            .CustomInstantiator(f => new UpdateTenantSubscriptionCommand(
                Id: id ?? Guid.NewGuid(),
                Plan: plan ?? f.PickRandom<PlanType>()
            ));

        return faker.Generate();
    }
}
