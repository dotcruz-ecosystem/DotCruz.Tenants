using DotCruz.Tenants.Application.Abstractions.Data;
using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;
using DotCruz.Tenants.Application.Mappers.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.UpdateTenantAddress;

public class UpdateTenantAddressCommandHandler : IRequestHandler<UpdateTenantAddressCommand>
{
    private readonly ITenantWriteRepository _tenantWriteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTenantAddressCommandHandler(
        ITenantWriteRepository tenantWriteRepository,
        IUnitOfWork unitOfWork)
    {
        _tenantWriteRepository = tenantWriteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateTenantAddressCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantWriteRepository.GetByIdToUpdateAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(ResourceMessagesException.TENANT_NOT_FOUND);
        
        var newAddress = request.TenantAddress.ToDomain();
        
        tenant.UpdateAddress(newAddress);

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
