using CommonTestUtilities.Commands.Tenants;
using CommonTestUtilities.Data;
using CommonTestUtilities.Data.Repositories.Tenants;
using CommonTestUtilities.Entities;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.AddTenantMember;
using DotCruz.Tenants.Domain.Entities.Tenants;
using DotCruz.Tenants.Domain.Enums.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;

namespace UseCases.Test.Tenants.AddTenantMember;

public class AddTenantMemberCommandHandlerTests
{
    [Fact]
    public async Task Success()
    {
        var tenant = new TenantBuilder().Build();
        var command = AddTenantMemberCommandBuilder.Build(tenantId: tenant.Id, email: "collab@dotcruz.com", role: TenantRole.Member);
        var handler = CreateHandler(tenant);

        var memberId = await handler.Handle(command, TestContext.Current.CancellationToken);

        Assert.NotEqual(Guid.Empty, memberId);
        var member = tenant.Members.FirstOrDefault(m => m.Id == memberId);
        Assert.NotNull(member);
        Assert.Equal("collab@dotcruz.com", member.Email.Value);
        Assert.Equal(TenantRole.Member, member.Role);
    }

    [Fact]
    public async Task Error_Tenant_Not_Found()
    {
        var command = AddTenantMemberCommandBuilder.Build();
        var handler = CreateHandler();

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.TENANT_NOT_FOUND, message);
    }

    [Fact]
    public async Task Error_Tenant_Inactive()
    {
        var tenant = new TenantBuilder().Build();
        tenant.Deactivate();

        var command = AddTenantMemberCommandBuilder.Build(tenantId: tenant.Id);
        var handler = CreateHandler(tenant);

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        Assert.Contains(exception.GetErrorsMessages(), m => m == ResourceMessagesException.TENANT_INACTIVE_CANNOT_ADD_MEMBER);
    }

    [Fact]
    public async Task Error_Duplicate_Email()
    {
        var tenant = new TenantBuilder().Build();
        var existingEmail = "duplicate@dotcruz.com";
        tenant.AddMember(DotCruz.Tenants.Domain.ValueObjects.Communication.Email.Create(existingEmail));

        var command = AddTenantMemberCommandBuilder.Build(tenantId: tenant.Id, email: existingEmail);
        var handler = CreateHandler(tenant);

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        Assert.Contains(exception.GetErrorsMessages(), m => m == ResourceMessagesException.MEMBER_ALREADY_EXISTS);
    }

    private static AddTenantMemberCommandHandler CreateHandler(Tenant? tenant = null)
    {
        var tenantWriteRepository = new TenantWriteRepositoryBuilder();

        if (tenant != null)
            tenantWriteRepository.GetByIdWithMembersToUpdate(tenant, TestContext.Current.CancellationToken);

        var unitOfWork = UnitOfWorkerBuilder.Build();

        return new AddTenantMemberCommandHandler(
            tenantWriteRepository.Build(),
            unitOfWork
        );
    }
}
