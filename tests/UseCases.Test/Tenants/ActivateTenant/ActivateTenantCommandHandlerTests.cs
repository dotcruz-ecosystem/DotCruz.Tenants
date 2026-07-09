using CommonTestUtilities.Commands.Tenants;
using CommonTestUtilities.Data;
using CommonTestUtilities.Data.Repositories.Tenants;
using CommonTestUtilities.Entities;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.ActivateTenant;
using DotCruz.Tenants.Domain.Entities.Tenants;
using DotCruz.Tenants.Domain.Enums.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;

namespace UseCases.Test.Tenants.ActivateTenant;

public class ActivateTenantCommandHandlerTests
{
    [Fact]
    public async Task Success()
    {
        var tenant = new TenantBuilder().Build();
        var command = ActivateTenantCommandBuilder.Build(id: tenant.Id);
        var handler = CreateHandler(tenant);

        await handler.Handle(command, TestContext.Current.CancellationToken);

        Assert.True(tenant.Status == TenantStatus.Active || tenant.Status == TenantStatus.Trialing);
        Assert.Null(tenant.SuspensionReason);
    }

    [Fact]
    public async Task Error_Tenant_Not_Found()
    {
        var command = ActivateTenantCommandBuilder.Build();
        var handler = CreateHandler();

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.TENANT_NOT_FOUND, message);
    }

    private static ActivateTenantCommandHandler CreateHandler(Tenant? tenant = null)
    {
        var tenantWriteRepository = new TenantWriteRepositoryBuilder();

        if (tenant != null)
            tenantWriteRepository.GetByIdToUpdate(tenant, TestContext.Current.CancellationToken);

        var unitOfWork = UnitOfWorkerBuilder.Build();

        return new ActivateTenantCommandHandler(
            tenantWriteRepository.Build(),
            unitOfWork
        );
    }
}
