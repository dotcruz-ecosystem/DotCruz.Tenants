using DotCruz.Tenants.Application.Abstractions.Data;
using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;
using DotCruz.Tenants.Domain.Entities.Tenants;
using DotCruz.Tenants.Domain.Enums.Tenants;
using DotCruz.Tenants.Domain.ValueObjects.Communication;
using DotCruz.Tenants.Domain.ValueObjects.Identity;
using DotCruz.Tenants.Domain.ValueObjects.Tenants;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.ProvisionPersonalWorkspace;

public class ProvisionPersonalWorkspaceCommandHandler : IRequestHandler<ProvisionPersonalWorkspaceCommand>
{
    private readonly ITenantWriteRepository _tenantWriteRepository;
    private readonly ITenantReadRepository _tenantReadRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProvisionPersonalWorkspaceCommandHandler> _logger;

    public ProvisionPersonalWorkspaceCommandHandler(
        ITenantWriteRepository tenantWriteRepository,
        ITenantReadRepository tenantReadRepository,
        IUnitOfWork unitOfWork,
        ILogger<ProvisionPersonalWorkspaceCommandHandler> logger)
    {
        _tenantWriteRepository = tenantWriteRepository;
        _tenantReadRepository = tenantReadRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(ProvisionPersonalWorkspaceCommand request, CancellationToken cancellationToken)
    {
        var existingWorkspaces = await _tenantReadRepository.GetUserWorkspacesAsync(request.UserId, cancellationToken);
        if (existingWorkspaces.Any(w => w.Tenant.Type == TenantType.Personal))
        {
            _logger.LogInformation("Personal workspace already exists for user {UserId}. Skipping provisioning.", request.UserId);
            return;
        }

        var slugValue = $"personal-{request.UserId:N}";
        var slug = TenantSlug.Create(slugValue);

        var workspaceName = string.IsNullOrWhiteSpace(request.Name)
            ? "Workspace Pessoal"
            : $"Workspace de {request.Name.Trim()}";

        if (workspaceName.Length > 100)
            workspaceName = workspaceName[..100];

        var name = Name.Create(workspaceName);
        var email = Email.Create(request.Email);

        var tenant = Tenant.CreateWorkspace(
            name: name,
            slug: slug,
            type: TenantType.Personal,
            ownerEmail: email,
            ownerUserId: request.UserId
        );

        await _tenantWriteRepository.AddAsync(tenant);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Personal workspace {TenantId} provisioned successfully for user {UserId}", tenant.Id, request.UserId);
    }
}
