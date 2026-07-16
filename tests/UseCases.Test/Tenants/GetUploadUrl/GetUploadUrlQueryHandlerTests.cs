using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;
using DotCruz.Tenants.Application.Abstractions.Services.Storage;
using DotCruz.Tenants.Application.Abstractions.Services.Storage.Responses;
using DotCruz.Tenants.Application.DTOs.Storage;
using DotCruz.Tenants.Application.UseCases.Tenants.Queries.GetUploadUrl;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Shared.Security.Context;
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
        var tenantId = Guid.NewGuid();
        var purpose = UploadPurpose.Branding;
        var folder = "branding";
        var fileName = "logo.png";
        var contentType = "image/png";
        var uploadUrl = "https://r2.com/upload";
        var publicUrl = "https://pub.com/logo.png";

        var tenantReadRepository = new Mock<ITenantReadRepository>();
        tenantReadRepository.Setup(x => x.ExistsAsync(tenantId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var storageService = new Mock<IStorageService>();
        storageService.Setup(x => x.GeneratePresignedUploadUrlAsync(tenantId, folder, fileName, contentType, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new StorageUploadUrlDto(uploadUrl, publicUrl));

        var securityContext = new Mock<ISecurityContext>();
        securityContext.Setup(x => x.TenantId).Returns(tenantId);

        var query = new GetUploadUrlQuery(tenantId, purpose, fileName, contentType);
        var handler = new GetUploadUrlQueryHandler(tenantReadRepository.Object, storageService.Object, securityContext.Object);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(uploadUrl, result.UploadUrl);
        Assert.Equal(publicUrl, result.PublicUrl);
    }

    [Fact]
    public async Task Error_Tenant_NotFound()
    {
        var tenantId = Guid.NewGuid();
        var purpose = UploadPurpose.Branding;
        var fileName = "logo.png";
        var contentType = "image/png";

        var tenantReadRepository = new Mock<ITenantReadRepository>();
        tenantReadRepository.Setup(x => x.ExistsAsync(tenantId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var storageService = new Mock<IStorageService>();

        var securityContext = new Mock<ISecurityContext>();
        securityContext.Setup(x => x.TenantId).Returns(tenantId);

        var query = new GetUploadUrlQuery(tenantId, purpose, fileName, contentType);
        var handler = new GetUploadUrlQueryHandler(tenantReadRepository.Object, storageService.Object, securityContext.Object);

        var act = () => handler.Handle(query, CancellationToken.None);

        await Assert.ThrowsAsync<NotFoundException>(act);
    }

    [Fact]
    public async Task Error_Invalid_Purpose()
    {
        var tenantId = Guid.NewGuid();
        var invalidPurpose = (UploadPurpose)999;
        var fileName = "logo.png";
        var contentType = "image/png";

        var tenantReadRepository = new Mock<ITenantReadRepository>();
        tenantReadRepository.Setup(x => x.ExistsAsync(tenantId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var storageService = new Mock<IStorageService>();

        var securityContext = new Mock<ISecurityContext>();
        securityContext.Setup(x => x.TenantId).Returns(tenantId);

        var query = new GetUploadUrlQuery(tenantId, invalidPurpose, fileName, contentType);
        var handler = new GetUploadUrlQueryHandler(tenantReadRepository.Object, storageService.Object, securityContext.Object);

        var act = () => handler.Handle(query, CancellationToken.None);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
    }
}
