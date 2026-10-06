using DotCruz.Tenants.Application.Abstractions.Data;
using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.RemoveTenantMember;

public class RemoveTenantMemberCommandHandler : IRequestHandler<RemoveTenantMemberCommand>
{
    private readonly ITenantWriteRepository _tenantWriteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveTenantMemberCommandHandler(
        ITenantWriteRepository tenantWriteRepository,
        IUnitOfWork unitOfWork)
    {
        _tenantWriteRepository = tenantWriteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RemoveTenantMemberCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantWriteRepository.GetByIdWithMembersToUpdateAsync(request.TenantId, cancellationToken)
            ?? throw new NotFoundException(ResourceMessagesException.TENANT_NOT_FOUND);

        tenant.RemoveMember(request.MemberId);

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
