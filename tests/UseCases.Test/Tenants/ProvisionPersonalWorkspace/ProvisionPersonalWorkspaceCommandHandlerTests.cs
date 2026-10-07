using CommonTestUtilities.Commands.Tenants;
using CommonTestUtilities.Data;
using CommonTestUtilities.Data.Repositories.Tenants;
using CommonTestUtilities.Services;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.ProvisionPersonalWorkspace;
using DotCruz.Tenants.Domain.Entities.Tenants;
using DotCruz.Tenants.Domain.Enums.Tenants;
using DotCruz.Tenants.Domain.ValueObjects.Communication;
using DotCruz.Tenants.Domain.ValueObjects.Identity;
using DotCruz.Tenants.Domain.ValueObjects.Tenants;
using Moq;

namespace UseCases.Test.Tenants.ProvisionPersonalWorkspace;

public class ProvisionPersonalWorkspaceCommandHandlerTests
{
    [Fact]
    public async Task Success_ProvisionPersonalWorkspace()
    {
        var userId = Guid.NewGuid();
        var command = ProvisionPersonalWorkspaceCommandBuilder.Build(
            userId: userId,
            name: "João Silva",
            email: "joao.silva@example.com"
        );

        var writeRepoBuilder = new TenantWriteRepositoryBuilder();
        var readRepoBuilder = new TenantReadRepositoryBuilder();
        readRepoBuilder.GetUserWorkspacesAsync(userId, [], TestContext.Current.CancellationToken);

        var unitOfWorkBuilder = new UnitOfWorkerBuilder();
        var logger = LoggerBuilder.Build<ProvisionPersonalWorkspaceCommandHandler>();

        var handler = new ProvisionPersonalWorkspaceCommandHandler(
            writeRepoBuilder.Build(),
            readRepoBuilder.Build(),
            unitOfWorkBuilder.BuildInstance(),
            logger
        );

        await handler.Handle(command, TestContext.Current.CancellationToken);

        writeRepoBuilder.Mock.Verify(r => r.AddAsync(
            It.Is<Tenant>(t => 
                t.Type == TenantType.Personal &&
                t.Status == TenantStatus.Active &&
                t.Slug.Value == $"personal-{userId:N}" &&
                t.Members.Any(m => m.UserId == userId && m.Role == TenantRole.Owner && m.Status == MemberStatus.Active)
            )
        ), Times.Once);

        unitOfWorkBuilder.Mock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Success_Idempotent_When_PersonalWorkspaceAlreadyExists()
    {
        var userId = Guid.NewGuid();
        var command = ProvisionPersonalWorkspaceCommandBuilder.Build(
            userId: userId,
            name: "Maria Santos",
            email: "maria.santos@example.com"
        );

        var existingTenant = Tenant.CreateWorkspace(
            Name.Create("Workspace de Maria"),
            TenantSlug.Create($"personal-{userId:N}"),
            TenantType.Personal,
            ownerEmail: Email.Create(command.Email),
            ownerUserId: userId
        );
        var member = existingTenant.Members.First();

        var writeRepoBuilder = new TenantWriteRepositoryBuilder();
        var readRepoBuilder = new TenantReadRepositoryBuilder();
        readRepoBuilder.GetUserWorkspacesAsync(userId, [(existingTenant, member)], TestContext.Current.CancellationToken);

        var unitOfWorkBuilder = new UnitOfWorkerBuilder();
        var logger = LoggerBuilder.Build<ProvisionPersonalWorkspaceCommandHandler>();

        var handler = new ProvisionPersonalWorkspaceCommandHandler(
            writeRepoBuilder.Build(),
            readRepoBuilder.Build(),
            unitOfWorkBuilder.BuildInstance(),
            logger
        );

        await handler.Handle(command, TestContext.Current.CancellationToken);

        writeRepoBuilder.Mock.Verify(r => r.AddAsync(It.IsAny<Tenant>()), Times.Never);
        unitOfWorkBuilder.Mock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Success_Sanitizes_And_Truncates_Long_Name()
    {
        var userId = Guid.NewGuid();
        var veryLongName = new string('A', 120);
        var command = ProvisionPersonalWorkspaceCommandBuilder.Build(
            userId: userId,
            name: veryLongName,
            email: "longname@example.com"
        );

        var writeRepoBuilder = new TenantWriteRepositoryBuilder();
        var readRepoBuilder = new TenantReadRepositoryBuilder();
        readRepoBuilder.GetUserWorkspacesAsync(userId, [], TestContext.Current.CancellationToken);

        var unitOfWorkBuilder = new UnitOfWorkerBuilder();
        var logger = LoggerBuilder.Build<ProvisionPersonalWorkspaceCommandHandler>();

        var handler = new ProvisionPersonalWorkspaceCommandHandler(
            writeRepoBuilder.Build(),
            readRepoBuilder.Build(),
            unitOfWorkBuilder.BuildInstance(),
            logger
        );

        await handler.Handle(command, TestContext.Current.CancellationToken);

        writeRepoBuilder.Mock.Verify(r => r.AddAsync(
            It.Is<Tenant>(t => t.Name.Value.Length <= 100)
        ), Times.Once);

        unitOfWorkBuilder.Mock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
