namespace DotCruz.Tenants.Application.DTOs.Storage;

public sealed record GetUploadUrlRequest(
    string FileName,
    string ContentType,
    UploadPurpose Purpose
);
