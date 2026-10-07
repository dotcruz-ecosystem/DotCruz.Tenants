using Bogus;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.UpdateWorkspaceSubscription;
using DotCruz.Workspaces.Domain.Enums.Workspaces;

namespace CommonTestUtilities.Commands.Tenants;

public class UpdateWorkspaceSubscriptionCommandBuilder
{
    public static UpdateWorkspaceSubscriptionCommand Build(
        Guid? id = null,
        PlanType? plan = null
    )
    {
        var faker = new Faker<UpdateWorkspaceSubscriptionCommand>()
            .CustomInstantiator(f => new UpdateWorkspaceSubscriptionCommand(
                Id: id ?? Guid.NewGuid(),
                Plan: plan ?? f.PickRandom<PlanType>()
            ));

        return faker.Generate();
    }
}
