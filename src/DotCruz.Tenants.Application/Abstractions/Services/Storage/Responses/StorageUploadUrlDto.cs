namespace DotCruz.Tenants.Application.Abstractions.Services.Storage.Responses;

public sealed record StorageUploadUrlDto(
    string UploadUrl,
    string PublicUrl
);