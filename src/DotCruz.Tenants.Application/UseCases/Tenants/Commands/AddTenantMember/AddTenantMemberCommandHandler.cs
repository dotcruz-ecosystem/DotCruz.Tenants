using DotCruz.Tenants.Application.Abstractions.Data;
using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using DotCruz.Tenants.Domain.ValueObjects.Communication;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.AddTenantMember;

public class AddTenantMemberCommandHandler : IRequestHandler<AddTenantMemberCommand, Guid>
{
    private readonly ITenantWriteRepository _tenantWriteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddTenantMemberCommandHandler(
        ITenantWriteRepository tenantWriteRepository,
        IUnitOfWork unitOfWork)
    {
        _tenantWriteRepository = tenantWriteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(AddTenantMemberCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantWriteRepository.GetByIdWithMembersToUpdateAsync(request.TenantId, cancellationToken)
            ?? throw new NotFoundException(ResourceMessagesException.TENANT_NOT_FOUND);

        var email = Email.Create(request.Email);
        var member = tenant.AddMember(email, request.Role, request.UserId);

        await _unitOfWork.CommitAsync(cancellationToken);

        return member.Id;
    }
}
