using CommonTestUtilities.Commands.Tenants;
using CommonTestUtilities.Data;
using CommonTestUtilities.Data.Repositories.Tenants;
using CommonTestUtilities.Entities;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.RemoveTenantMember;
using DotCruz.Tenants.Domain.Entities.Tenants;
using DotCruz.Tenants.Domain.Enums.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using DotCruz.Tenants.Domain.ValueObjects.Communication;

namespace UseCases.Test.Tenants.RemoveTenantMember;

public class RemoveTenantMemberCommandHandlerTests
{
    [Fact]
    public async Task Success()
    {
        var tenant = new TenantBuilder().Build();
        var member = tenant.AddMember(Email.Create("member@dotcruz.com"), TenantRole.Member);

        var command = RemoveTenantMemberCommandBuilder.Build(tenantId: tenant.Id, memberId: member.Id);
        var handler = CreateHandler(tenant);

        await handler.Handle(command, TestContext.Current.CancellationToken);

        Assert.NotNull(member.DeletedAt);
    }

    [Fact]
    public async Task Error_Tenant_Not_Found()
    {
        var command = RemoveTenantMemberCommandBuilder.Build();
        var handler = CreateHandler();

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.TENANT_NOT_FOUND, message);
    }

    [Fact]
    public async Task Error_Member_Not_Found()
    {
        var tenant = new TenantBuilder().Build();
        var command = RemoveTenantMemberCommandBuilder.Build(tenantId: tenant.Id, memberId: Guid.NewGuid());
        var handler = CreateHandler(tenant);

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.MEMBER_NOT_FOUND, message);
    }

    [Fact]
    public async Task Error_Cannot_Remove_Last_Owner()
    {
        var tenant = new TenantBuilder().WithOwnerMember().Build();
        var owner = tenant.Members.First(m => m.Role == TenantRole.Owner);

        var command = RemoveTenantMemberCommandBuilder.Build(tenantId: tenant.Id, memberId: owner.Id);
        var handler = CreateHandler(tenant);

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        Assert.Contains(exception.GetErrorsMessages(), m => m == ResourceMessagesException.CANNOT_REMOVE_LAST_OWNER);
    }

    private static RemoveTenantMemberCommandHandler CreateHandler(Tenant? tenant = null)
    {
        var tenantWriteRepository = new TenantWriteRepositoryBuilder();

        if (tenant != null)
            tenantWriteRepository.GetByIdWithMembersToUpdate(tenant, TestContext.Current.CancellationToken);

        var unitOfWork = UnitOfWorkerBuilder.Build();

        return new RemoveTenantMemberCommandHandler(
            tenantWriteRepository.Build(),
            unitOfWork
        );
    }
}
