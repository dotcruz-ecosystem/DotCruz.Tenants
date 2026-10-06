using CommonTestUtilities.Commands.Tenants;
using CommonTestUtilities.Data;
using CommonTestUtilities.Data.Repositories.Tenants;
using CommonTestUtilities.Security;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.CreateWorkspace;
using DotCruz.Tenants.Domain.Enums.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;

namespace UseCases.Test.Tenants.CreateWorkspace;

public class CreateWorkspaceCommandHandlerTests
{
    [Fact]
    public async Task Success_With_OwnerEmail()
    {
        var userId = Guid.NewGuid();
        var command = CreateWorkspaceCommandBuilder.Build(ownerEmail: "owner@dotcruz.com");
        var handler = CreateHandler(userId: userId);

        var result = await handler.Handle(command, TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.TenantId);
        Assert.Equal(command.Name, result.TenantName);
        Assert.Equal(command.Slug, result.TenantSlug);
        Assert.Equal(command.Type.ToString(), result.TenantType);
        Assert.Equal(TenantStatus.Active.ToString(), result.TenantStatus);
        Assert.Equal(TenantRole.Owner.ToString(), result.Role);
        Assert.Equal(MemberStatus.Active.ToString(), result.MemberStatus);
        Assert.Equal(PlanType.Trial.ToString(), result.Plan);
    }

    [Fact]
    public async Task Success_Without_OwnerEmail()
    {
        var userId = Guid.NewGuid();
        var command = CreateWorkspaceCommandBuilder.Build(ownerEmail: null);
        var handler = CreateHandler(userId: userId);

        var result = await handler.Handle(command, TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.TenantId);
        Assert.Equal(command.Name, result.TenantName);
        Assert.Equal(command.Slug, result.TenantSlug);
        Assert.Equal(TenantRole.Owner.ToString(), result.Role);
    }

    [Fact]
    public async Task Error_Unauthenticated_User()
    {
        var command = CreateWorkspaceCommandBuilder.Build();
        var handler = CreateHandler(userId: null);

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<ForbiddenException>(act);
        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE, message);
    }

    [Fact]
    public async Task Error_Slug_Already_Exists()
    {
        var userId = Guid.NewGuid();
        var command = CreateWorkspaceCommandBuilder.Build();
        var handler = CreateHandler(userId: userId, existingSlug: command.Slug);

        Task act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.SLUG_ALREADY_EXISTS, message);
    }

    private static CreateWorkspaceCommandHandler CreateHandler(Guid? userId = null, string? existingSlug = null)
    {
        var writeRepo = new TenantWriteRepositoryBuilder().Build();
        var readRepoBuilder = new TenantReadRepositoryBuilder();
        var unitOfWork = UnitOfWorkerBuilder.Build();
        var securityContextBuilder = new SecurityContextBuilder();

        if (userId.HasValue)
            securityContextBuilder.AsUser(userId.Value);

        if (!string.IsNullOrEmpty(existingSlug))
            readRepoBuilder.ExistsWithSlugAsync(existingSlug, TestContext.Current.CancellationToken);

        return new CreateWorkspaceCommandHandler(
            writeRepo,
            readRepoBuilder.Build(),
            unitOfWork,
            securityContextBuilder.Build()
        );
    }
}
