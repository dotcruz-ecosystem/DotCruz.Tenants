using CommonTestUtilities.Commands.Tenants;
using CommonTestUtilities.Data;
using CommonTestUtilities.Data.Repositories.Tenants;
using CommonTestUtilities.Entities;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.UpdateTenantAddress;
using DotCruz.Tenants.Domain.Entities.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;

namespace UseCases.Test.Tenants.UpdateTenantAddress;

public class UpdateTenantAddressCommandHandlerTests
{
    [Fact]
    public async Task Success()
    {
        var tenant = new TenantBuilder().Build();
        var command = UpdateTenantAddressCommandBuilder.Build(id: tenant.Id);
        var handler = CreateHandler(tenant);

        await handler.Handle(command, TestContext.Current.CancellationToken);

        Assert.Equal(command.TenantAddress.Street, tenant.Address.Street);
        Assert.Equal(command.TenantAddress.Number, tenant.Address.Number);
        Assert.Equal(command.TenantAddress.Complement, tenant.Address.Complement);
        Assert.Equal(command.TenantAddress.Neighborhood, tenant.Address.Neighborhood);
        Assert.Equal(command.TenantAddress.City, tenant.Address.City);
        Assert.Equal(command.TenantAddress.State, tenant.Address.State.Value);
        Assert.Equal(command.TenantAddress.ZipCode.Replace("-", ""), tenant.Address.ZipCode.Value);
    }

    [Fact]
    public async Task Error_Tenant_Not_Found()
    {
        var command = UpdateTenantAddressCommandBuilder.Build();
        var handler = CreateHandler();

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.TENANT_NOT_FOUND, message);
    }

    private static UpdateTenantAddressCommandHandler CreateHandler(Tenant? tenant = null)
    {
        var tenantWriteRepository = new TenantWriteRepositoryBuilder();

        if (tenant != null)
            tenantWriteRepository.GetByIdToUpdate(tenant, TestContext.Current.CancellationToken);

        var unitOfWork = UnitOfWorkerBuilder.Build();

        return new UpdateTenantAddressCommandHandler(
            tenantWriteRepository.Build(),
            unitOfWork
        );
    }
}
