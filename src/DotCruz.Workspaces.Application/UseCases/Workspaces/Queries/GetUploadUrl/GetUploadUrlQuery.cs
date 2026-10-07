using DotCruz.Workspaces.Application.Abstractions.Services.Storage;
using DotCruz.Workspaces.Application.Abstractions.Services.Storage.Responses;
using DotCruz.Workspaces.Application.DTOs.Storage;
using MediatR;
using System;
using DotCruz.Workspaces.Application.Abstractions.Security;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetUploadUrl;

public sealed record GetUploadUrlQuery(
    Guid WorkspaceId,
    UploadPurpose Purpose,
    string FileName,
    string ContentType
) : IRequest<StorageUploadUrlDto>, IWorkspaceScopedRequest;
