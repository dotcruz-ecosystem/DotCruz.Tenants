using CommonTestUtilities.Commands.Tenants;
using CommonTestUtilities.Data;
using CommonTestUtilities.Data.Repositories.Tenants;
using CommonTestUtilities.Entities;
using DotCruz.Tenants.Application.UseCases.Tenants.UpdateTenantSubscription;
using DotCruz.Tenants.Domain.Entities.Tenants;
using DotCruz.Tenants.Domain.Enums.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;

namespace UseCases.Test.Tenants.UpdateTenantSubscription;

public class UpdateTenantSubscriptionCommandHandlerTests
{
    [Fact]
    public async Task Success()
    {
        var tenant = new TenantBuilder().Build();
        var command = UpdateTenantSubscriptionCommandBuilder.Build(id: tenant.Id, plan: PlanType.Pro);
        var handler = CreateHandler(tenant);

        await handler.Handle(command, TestContext.Current.CancellationToken);

        Assert.Equal(command.Plan, tenant.Subscription.Plan);
        Assert.Equal(50, tenant.Subscription.Limits.MaxUsers);
        Assert.Equal(50000, tenant.Subscription.Limits.MaxEmailsPerMonth);
    }

    [Fact]
    public async Task Error_Tenant_Not_Found()
    {
        var command = UpdateTenantSubscriptionCommandBuilder.Build();
        var handler = CreateHandler();

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.TENANT_NOT_FOUND, message);
    }

    private static UpdateTenantSubscriptionCommandHandler CreateHandler(Tenant? tenant = null)
    {
        var tenantWriteRepository = new TenantWriteRepositoryBuilder();

        if (tenant != null)
            tenantWriteRepository.GetByIdToUpdate(tenant, TestContext.Current.CancellationToken);

        var unitOfWork = UnitOfWorkerBuilder.Build();

        return new UpdateTenantSubscriptionCommandHandler(
            tenantWriteRepository.Build(),
            unitOfWork
        );
    }
}
