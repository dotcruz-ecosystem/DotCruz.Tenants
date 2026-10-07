using DotCruz.Workspaces.Application.Abstractions.Data.Repositories.Workspaces;
using DotCruz.Workspaces.Application.Abstractions.Services.Storage;
using DotCruz.Workspaces.Application.Abstractions.Services.Storage.Responses;
using DotCruz.Workspaces.Application.DTOs.Storage;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetUploadUrl;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UseCases.Test.Tenants.GetUploadUrl;

public class GetUploadUrlQueryHandlerTests
{
    [Fact]
    public async Task Success()
    {
        var WorkspaceId = Guid.NewGuid();
        var purpose = UploadPurpose.Branding;
        var folder = "branding";
        var fileName = "logo.png";
        var contentType = "image/png";
        var uploadUrl = "https://r2.com/upload";
        var publicUrl = "https://pub.com/logo.png";

        var WorkspaceReadRepository = new Mock<IWorkspaceReadRepository>();
        WorkspaceReadRepository.Setup(x => x.ExistsAsync(WorkspaceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var storageService = new Mock<IStorageService>();
        storageService.Setup(x => x.GeneratePresignedUploadUrlAsync(WorkspaceId, folder, fileName, contentType, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new StorageUploadUrlDto(uploadUrl, publicUrl));

        var query = new GetUploadUrlQuery(WorkspaceId, purpose, fileName, contentType);
        var handler = new GetUploadUrlQueryHandler(WorkspaceReadRepository.Object, storageService.Object);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(uploadUrl, result.UploadUrl);
        Assert.Equal(publicUrl, result.PublicUrl);
    }

    [Fact]
    public async Task Error_Tenant_NotFound()
    {
        var WorkspaceId = Guid.NewGuid();
        var purpose = UploadPurpose.Branding;
        var fileName = "logo.png";
        var contentType = "image/png";

        var WorkspaceReadRepository = new Mock<IWorkspaceReadRepository>();
        WorkspaceReadRepository.Setup(x => x.ExistsAsync(WorkspaceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var storageService = new Mock<IStorageService>();

        var query = new GetUploadUrlQuery(WorkspaceId, purpose, fileName, contentType);
        var handler = new GetUploadUrlQueryHandler(WorkspaceReadRepository.Object, storageService.Object);

        var act = () => handler.Handle(query, CancellationToken.None);

        await Assert.ThrowsAsync<NotFoundException>(act);
    }

    [Fact]
    public async Task Error_Invalid_Purpose()
    {
        var WorkspaceId = Guid.NewGuid();
        var invalidPurpose = (UploadPurpose)999;
        var fileName = "logo.png";
        var contentType = "image/png";

        var WorkspaceReadRepository = new Mock<IWorkspaceReadRepository>();
        WorkspaceReadRepository.Setup(x => x.ExistsAsync(WorkspaceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var storageService = new Mock<IStorageService>();

        var query = new GetUploadUrlQuery(WorkspaceId, invalidPurpose, fileName, contentType);
        var handler = new GetUploadUrlQueryHandler(WorkspaceReadRepository.Object, storageService.Object);

        var act = () => handler.Handle(query, CancellationToken.None);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
    }
}
