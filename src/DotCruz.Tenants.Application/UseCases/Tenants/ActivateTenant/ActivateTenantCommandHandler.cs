using DotCruz.Tenants.Application.Abstractions.Data;
using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.ActivateTenant;

public class ActivateTenantCommandHandler : IRequestHandler<ActivateTenantCommand>
{
    private readonly ITenantWriteRepository _tenantWriteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateTenantCommandHandler(ITenantWriteRepository tenantWriteRepository, IUnitOfWork unitOfWork)
    {
        _tenantWriteRepository = tenantWriteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ActivateTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantWriteRepository.GetByIdToUpdateAsync(request.Id, cancellationToken) 
            ?? throw new NotFoundException(ResourceMessagesException.TENANT_NOT_FOUND);
        
        tenant.Activate();

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
