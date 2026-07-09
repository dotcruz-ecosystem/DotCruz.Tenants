using DotCruz.Tenants.Application.Abstractions.Data;
using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.TerminateTenant;

public class TerminateTenantCommandHandler : IRequestHandler<TerminateTenantCommand>
{
    private readonly ITenantWriteRepository _tenantWriteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TerminateTenantCommandHandler(ITenantWriteRepository tenantWriteRepository, IUnitOfWork unitOfWork)
    {
        _tenantWriteRepository = tenantWriteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(TerminateTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantWriteRepository.GetByIdToUpdateAsync(request.Id, cancellationToken) 
            ?? throw new NotFoundException(ResourceMessagesException.TENANT_NOT_FOUND);
        
        tenant.Terminate();

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
