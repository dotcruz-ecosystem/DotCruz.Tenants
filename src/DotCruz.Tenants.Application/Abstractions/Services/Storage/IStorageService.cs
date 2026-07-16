using DotCruz.Tenants.Application.Abstractions.Services.Storage.Responses;

namespace DotCruz.Tenants.Application.Abstractions.Services.Storage;

public interface IStorageService
{
    Task<StorageUploadUrlDto> GeneratePresignedUploadUrlAsync(
        Guid tenantId,
        string folder,
        string fileName,
        string contentType,
        CancellationToken cancellationToken);
}
