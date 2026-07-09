using CommonTestUtilities.Commands.Tenants;
using CommonTestUtilities.Data;
using CommonTestUtilities.Data.Repositories.Tenants;
using CommonTestUtilities.Entities;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.UpdateTenantContact;
using DotCruz.Tenants.Domain.Entities.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;

namespace UseCases.Test.Tenants.UpdateTenantContact;

public class UpdateTenantContactCommandHandlerTests
{
    [Fact]
    public async Task Success()
    {
        var tenant = new TenantBuilder().Build();
        var command = UpdateTenantContactCommandBuilder.Build(id: tenant.Id);
        var handler = CreateHandler(tenant);

        await handler.Handle(command, TestContext.Current.CancellationToken);

        Assert.Equal(command.TenantContact.Email.ToLowerInvariant(), tenant.Contact.Email.Value);
        Assert.Equal(command.TenantContact.PhoneCountryCode, tenant.Contact.Phone.CountryCode);
        Assert.Equal(command.TenantContact.PhoneNationalNumber, tenant.Contact.Phone.NationalNumber);
    }

    [Fact]
    public async Task Error_Tenant_Not_Found()
    {
        var command = UpdateTenantContactCommandBuilder.Build();
        var handler = CreateHandler();

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.TENANT_NOT_FOUND, message);
    }

    private static UpdateTenantContactCommandHandler CreateHandler(Tenant? tenant = null)
    {
        var tenantWriteRepository = new TenantWriteRepositoryBuilder();

        if (tenant != null)
            tenantWriteRepository.GetByIdToUpdate(tenant, TestContext.Current.CancellationToken);

        var unitOfWork = UnitOfWorkerBuilder.Build();

        return new UpdateTenantContactCommandHandler(
            tenantWriteRepository.Build(),
            unitOfWork
        );
    }
}
