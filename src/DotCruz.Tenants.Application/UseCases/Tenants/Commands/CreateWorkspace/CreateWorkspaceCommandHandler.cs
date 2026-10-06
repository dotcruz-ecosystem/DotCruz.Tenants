using DotCruz.Shared.Security.Context;
using DotCruz.Tenants.Application.Abstractions.Data;
using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;
using DotCruz.Tenants.Application.DTOs.Tenants;
using DotCruz.Tenants.Domain.Entities.Tenants;
using DotCruz.Tenants.Domain.Enums.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using DotCruz.Tenants.Domain.ValueObjects.Communication;
using DotCruz.Tenants.Domain.ValueObjects.Identity;
using DotCruz.Tenants.Domain.ValueObjects.Tenants;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.CreateWorkspace;

public class CreateWorkspaceCommandHandler : IRequestHandler<CreateWorkspaceCommand, UserWorkspaceDto>
{
    private readonly ITenantWriteRepository _tenantWriteRepository;
    private readonly ITenantReadRepository _tenantReadRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISecurityContext _securityContext;

    public CreateWorkspaceCommandHandler(
        ITenantWriteRepository tenantWriteRepository,
        ITenantReadRepository tenantReadRepository,
        IUnitOfWork unitOfWork,
        ISecurityContext securityContext)
    {
        _tenantWriteRepository = tenantWriteRepository;
        _tenantReadRepository = tenantReadRepository;
        _unitOfWork = unitOfWork;
        _securityContext = securityContext;
    }

    public async Task<UserWorkspaceDto> Handle(CreateWorkspaceCommand request, CancellationToken cancellationToken)
    {
        if (!_securityContext.UserId.HasValue)
            throw new ForbiddenException(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE);

        if (await _tenantReadRepository.ExistsWithSlugAsync(request.Slug, cancellationToken))
            throw new ErrorOnValidationException(ResourceMessagesException.SLUG_ALREADY_EXISTS);

        var ownerEmail = !string.IsNullOrWhiteSpace(request.OwnerEmail)
            ? Email.Create(request.OwnerEmail)
            : Email.Create($"user_{_securityContext.UserId.Value:N}@dotcruz.com");

        var tenant = Tenant.CreateWorkspace(
            Name.Create(request.Name),
            TenantSlug.Create(request.Slug),
            request.Type,
            ownerEmail: ownerEmail,
            ownerUserId: _securityContext.UserId.Value
        );

        await _tenantWriteRepository.AddAsync(tenant);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new UserWorkspaceDto(
            tenant.Id,
            tenant.Name.Value,
            tenant.Slug.Value,
            tenant.Type.ToString(),
            tenant.Status.ToString(),
            TenantRole.Owner.ToString(),
            MemberStatus.Active.ToString(),
            tenant.Subscription.Plan.ToString()
        );
    }
}
