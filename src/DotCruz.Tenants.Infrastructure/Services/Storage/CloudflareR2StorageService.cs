using Amazon.S3;
using Amazon.S3.Model;
using DotCruz.Tenants.Application.Abstractions.Services.Storage;
using DotCruz.Tenants.Application.Abstractions.Services.Storage.Responses;
using Microsoft.Extensions.Options;

namespace DotCruz.Tenants.Infrastructure.Services.Storage;

public class CloudflareR2StorageService : IStorageService
{
    private readonly CloudflareR2Settings _settings;

    public CloudflareR2StorageService(IOptions<CloudflareR2Settings> settings)
    {
        _settings = settings.Value;
    }

    public async Task<StorageUploadUrlDto> GeneratePresignedUploadUrlAsync(
        Guid tenantId,
        string folder,
        string fileName,
        string contentType,
        CancellationToken cancellationToken)
    {
        var s3Config = new AmazonS3Config
        {
            ServiceURL = $"https://{_settings.AccountId}.r2.cloudflarestorage.com",
            ForcePathStyle = true
        };

        using var s3Client = new AmazonS3Client(_settings.AccessKey, _settings.SecretKey, s3Config);

        var cleanFileName = Path.GetFileNameWithoutExtension(fileName);
        var extension = Path.GetExtension(fileName);
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var uniqueFileName = $"{cleanFileName}-{timestamp}{extension}";

        // Key pattern: tenants/{tenantId}/{folder}/{uniqueFileName}
        var objectKey = $"tenants/{tenantId}/{folder}/{uniqueFileName}";

        var request = new GetPreSignedUrlRequest
        {
            BucketName = _settings.BucketName,
            Key = objectKey,
            Verb = HttpVerb.PUT,
            ContentType = contentType,
            Expires = DateTime.UtcNow.AddMinutes(15) // URL valid for 15 minutes
        };

        var uploadUrl = await s3Client.GetPreSignedURLAsync(request);

        // Format final public access URL
        // Example: https://pub-xxx.r2.dev/tenants/tenant-id/branding/logo-xxx.png
        var publicBaseUrl = _settings.PublicBaseUrl.TrimEnd('/');
        var publicUrl = $"{publicBaseUrl}/{objectKey}";

        return new StorageUploadUrlDto(uploadUrl, publicUrl);
    }
}
