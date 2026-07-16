using DotCruz.Shared.Security.Context;
using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;
using DotCruz.Tenants.Application.Abstractions.Services.Storage;
using DotCruz.Tenants.Application.Abstractions.Services.Storage.Responses;
using DotCruz.Tenants.Application.DTOs.Storage;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Queries.GetUploadUrl;

public class GetUploadUrlQueryHandler : IRequestHandler<GetUploadUrlQuery, StorageUploadUrlDto>
{
    private readonly ITenantReadRepository _tenantReadRepository;
    private readonly IStorageService _storageService;
    private readonly ISecurityContext _securityContext;

    public GetUploadUrlQueryHandler(
        ITenantReadRepository tenantReadRepository,
        IStorageService storageService,
        ISecurityContext securityContext
    )
    {
        _tenantReadRepository = tenantReadRepository;
        _storageService = storageService;
        _securityContext = securityContext;
    }

    public async Task<StorageUploadUrlDto> Handle(GetUploadUrlQuery request, CancellationToken cancellationToken)
    {
        if (_securityContext.TenantId != request.TenantId)
            throw new ForbiddenException(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE);

        var tenantExists = await _tenantReadRepository.ExistsAsync(request.TenantId, cancellationToken);
        if (!tenantExists)
            throw new NotFoundException(ResourceMessagesException.TENANT_NOT_FOUND);

        var folder = request.Purpose switch
        {
            UploadPurpose.Branding => "branding",
            _ => throw new ErrorOnValidationException("Invalid upload purpose.")
        };

        return await _storageService.GeneratePresignedUploadUrlAsync(
            request.TenantId,
            folder,
            request.FileName,
            request.ContentType,
            cancellationToken);
    }
}
