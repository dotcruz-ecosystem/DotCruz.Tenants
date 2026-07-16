using DotCruz.Shared.Security.Context;
using DotCruz.Tenants.Application.Abstractions.Data;
using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;
using DotCruz.Tenants.Application.Mappers.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.UpdateTenantBranding;

public class UpdateTenantBrandingCommandHandler : IRequestHandler<UpdateTenantBrandingCommand>
{
    private readonly ITenantWriteRepository _tenantWriteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISecurityContext _securityContext;

    public UpdateTenantBrandingCommandHandler(
        ITenantWriteRepository tenantWriteRepository,
        IUnitOfWork unitOfWork,
        ISecurityContext securityContext
    )
    {
        _tenantWriteRepository = tenantWriteRepository;
        _unitOfWork = unitOfWork;
        _securityContext = securityContext;
    }

    public async Task Handle(UpdateTenantBrandingCommand request, CancellationToken cancellationToken)
    {
        if (_securityContext.TenantId != request.Id)
            throw new ForbiddenException(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE);

        var tenant = await _tenantWriteRepository.GetByIdToUpdateAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(ResourceMessagesException.TENANT_NOT_FOUND);

        var newBranding = request.TenantBranding.ToDomain();

        tenant.UpdateBranding(newBranding);

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
