using DotCruz.Tenants.Application.Abstractions.Services.Storage;
using DotCruz.Tenants.Application.Abstractions.Services.Storage.Responses;
using DotCruz.Tenants.Application.DTOs.Storage;
using MediatR;
using System;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Queries.GetUploadUrl;

public sealed record GetUploadUrlQuery(
    Guid TenantId,
    UploadPurpose Purpose,
    string FileName,
    string ContentType
) : IRequest<StorageUploadUrlDto>;
