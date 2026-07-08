using CommonTestUtilities.Commands.Tenants;
using CommonTestUtilities.Data;
using CommonTestUtilities.Data.Repositories.Tenants;
using DotCruz.Tenants.Application.Mappers.Fiscal;
using DotCruz.Tenants.Application.UseCases.Tenants.CreateTenant;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;

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
        var tenantWriteRepository = TenantWriteRepositoryBuilder.Build();
        var tenantReadRepository = new TenantReadRepositoryBuilder();
        var unitOfWork = UnitOfWorkerBuilder.Build();

        if (!string.IsNullOrEmpty(slug))
            tenantReadRepository.ExistsWithSlugAsync(slug, TestContext.Current.CancellationToken);

        if (!string.IsNullOrEmpty(document))
            tenantReadRepository.ExistsWithDocumentAsync(document, TestContext.Current.CancellationToken);

        return new CreateTenantCommandHandler(
            tenantWriteRepository,
            tenantReadRepository.Build(),
            unitOfWork
        );
    }
}
