using CommonTestUtilities.Commands.Tenants;
using CommonTestUtilities.Data;
using CommonTestUtilities.Data.Repositories.Workspaces;
using CommonTestUtilities.Services;
using DotCruz.Workspaces.Application.Mappers.Fiscal;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.CreateTenant;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;

namespace UseCases.Test.Tenants.CreateTenant;

public class CreateTenantCommandHandlerTests
{
    [Fact]
    public async Task Success()
    {
        var command = CreateTenantCommandBuilder.Build();
        var handler = CreateHandler();

        var result = await handler.Handle(command, TestContext.Current.CancellationToken);

        Assert.NotEqual(Guid.Empty, result);
    }

    [Fact]
    public async Task Error_Slug_Already_Exists()
    {
        var command = CreateTenantCommandBuilder.Build();
        var handler = CreateHandler(slug: command.Slug);

        Task<Guid> act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.SLUG_ALREADY_EXISTS, message);
    }

    [Fact]
    public async Task Error_Document_Already_Exists()
    {
        var command = CreateTenantCommandBuilder.Build();
        var handler = CreateHandler(document: command.TenantDocument.ToDomain().Number);

        Task<Guid> act() => handler.Handle(command, TestContext.Current.CancellationToken);

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.DOCUMENT_ALREADY_EXISTS, message);
    }

    private static CreateTenantCommandHandler CreateHandler(string? slug = null, string? document = null)
    {
        var WorkspaceWriteRepository = new WorkspaceWriteRepositoryBuilder().Build();
        var WorkspaceReadRepository = new WorkspaceReadRepositoryBuilder();
        var unitOfWork = UnitOfWorkerBuilder.Build();
        var mediator = MediatorBuilder.Build();

        if (!string.IsNullOrEmpty(slug))
            WorkspaceReadRepository.ExistsWithSlugAsync(slug, TestContext.Current.CancellationToken);

        if (!string.IsNullOrEmpty(document))
            WorkspaceReadRepository.ExistsWithDocumentAsync(document, TestContext.Current.CancellationToken);

        return new CreateTenantCommandHandler(
            WorkspaceWriteRepository,
            WorkspaceReadRepository.Build(),
            unitOfWork,
            mediator
        );
    }
}
