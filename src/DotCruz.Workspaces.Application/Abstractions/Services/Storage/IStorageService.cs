using DotCruz.Workspaces.Application.Abstractions.Services.Storage.Responses;

namespace DotCruz.Workspaces.Application.Abstractions.Services.Storage;

public interface IStorageService
{
    Task<StorageUploadUrlDto> GeneratePresignedUploadUrlAsync(
        Guid WorkspaceId,
        string folder,
        string fileName,
        string contentType,
        CancellationToken cancellationToken);
}
