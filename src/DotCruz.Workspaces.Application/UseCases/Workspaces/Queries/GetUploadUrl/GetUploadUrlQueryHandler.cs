using DotCruz.Workspaces.Application.Abstractions.Data.Repositories.Workspaces;
using DotCruz.Workspaces.Application.Abstractions.Services.Storage;
using DotCruz.Workspaces.Application.Abstractions.Services.Storage.Responses;
using DotCruz.Workspaces.Application.DTOs.Storage;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;
using MediatR;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetUploadUrl;

public class GetUploadUrlQueryHandler : IRequestHandler<GetUploadUrlQuery, StorageUploadUrlDto>
{
    private readonly IWorkspaceReadRepository _WorkspaceReadRepository;
    private readonly IStorageService _storageService;

    public GetUploadUrlQueryHandler(
        IWorkspaceReadRepository WorkspaceReadRepository,
        IStorageService storageService
    )
    {
        _WorkspaceReadRepository = WorkspaceReadRepository;
        _storageService = storageService;
    }

    public async Task<StorageUploadUrlDto> Handle(GetUploadUrlQuery request, CancellationToken cancellationToken)
    {
        var tenantExists = await _WorkspaceReadRepository.ExistsAsync(request.WorkspaceId, cancellationToken);
        if (!tenantExists)
            throw new NotFoundException(ResourceMessagesException.TENANT_NOT_FOUND);

        var folder = request.Purpose switch
        {
            UploadPurpose.Branding => "branding",
            _ => throw new ErrorOnValidationException("Invalid upload purpose.")
        };

        return await _storageService.GeneratePresignedUploadUrlAsync(
            request.WorkspaceId,
            folder,
            request.FileName,
            request.ContentType,
            cancellationToken);
    }
}
