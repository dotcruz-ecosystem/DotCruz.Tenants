using CommonTestUtilities.Commands.Tenants;
using CommonTestUtilities.Data;
using CommonTestUtilities.Data.Repositories.Tenants;
using CommonTestUtilities.Entities;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.DeactivateTenant;
using DotCruz.Tenants.Domain.Entities.Tenants;
using DotCruz.Tenants.Domain.Enums.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;

namespace UseCases.Test.Tenants.DeactivateTenant;

public class DeactivateTenantCommandHandlerTests
{
    [Fact]
    public async Task Success()
    {
        var tenant = new TenantBuilder().Build();
        var command = DeactivateTenantCommandBuilder.Build(id: tenant.Id);
        var handler = CreateHandler(tenant);

        await handler.Handle(command, TestContext.Current.CancellationToken);

        Assert.Equal(TenantStatus.Inactive, tenant.Status);
    }

    [Fact]
    public async Task Error_Tenant_Not_Found()
    {
        var command = DeactivateTenantCommandBuilder.Build();
        var handler = CreateHandler();

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.TENANT_NOT_FOUND, message);
    }

    private static DeactivateTenantCommandHandler CreateHandler(Tenant? tenant = null)
    {
        var tenantWriteRepository = new TenantWriteRepositoryBuilder();

        if (tenant != null)
            tenantWriteRepository.GetByIdToUpdate(tenant, TestContext.Current.CancellationToken);

        var unitOfWork = UnitOfWorkerBuilder.Build();

        return new DeactivateTenantCommandHandler(
            tenantWriteRepository.Build(),
            unitOfWork
        );
    }
}
