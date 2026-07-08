using DotCruz.Tenants.Application.Abstractions.Data;
using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using DotCruz.Tenants.Domain.ValueObjects.Tenants;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.SuspendTenant;

public class SuspendTenantCommandHandler : IRequestHandler<SuspendTenantCommand>
{
    private readonly ITenantWriteRepository _tenantWriteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SuspendTenantCommandHandler(ITenantWriteRepository tenantWriteRepository, IUnitOfWork unitOfWork)
    {
        _tenantWriteRepository = tenantWriteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(SuspendTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantWriteRepository.GetByIdToUpdateAsync(request.Id, cancellationToken) 
            ?? throw new NotFoundException(ResourceMessagesException.TENANT_NOT_FOUND);
        
        var reason = SuspensionReason.Create(request.Reason);
        
        tenant.Suspend(reason);

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
