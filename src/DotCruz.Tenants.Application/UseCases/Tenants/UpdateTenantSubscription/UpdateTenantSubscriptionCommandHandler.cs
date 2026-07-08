using DotCruz.Tenants.Application.Abstractions.Data;
using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.UpdateTenantSubscription;

public class UpdateTenantSubscriptionCommandHandler : IRequestHandler<UpdateTenantSubscriptionCommand>
{
    private readonly ITenantWriteRepository _tenantWriteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTenantSubscriptionCommandHandler(
        ITenantWriteRepository tenantWriteRepository,
        IUnitOfWork unitOfWork)
    {
        _tenantWriteRepository = tenantWriteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateTenantSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantWriteRepository.GetByIdToUpdateAsync(request.Id, cancellationToken) 
            ?? throw new NotFoundException(ResourceMessagesException.TENANT_NOT_FOUND);
        
        var upgradedSubscription = tenant.Subscription.Upgrade(request.Plan);
        
        tenant.UpdateSubscription(upgradedSubscription);

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
