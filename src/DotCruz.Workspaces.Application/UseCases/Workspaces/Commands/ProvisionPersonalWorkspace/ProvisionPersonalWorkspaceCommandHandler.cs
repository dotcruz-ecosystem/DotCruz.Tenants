using DotCruz.Workspaces.Application.Abstractions.Data;
using DotCruz.Workspaces.Application.Abstractions.Data.Repositories.Workspaces;
using DotCruz.Workspaces.Domain.Entities.Workspaces;
using DotCruz.Workspaces.Domain.Enums.Workspaces;
using DotCruz.Workspaces.Domain.ValueObjects.Communication;
using DotCruz.Workspaces.Domain.ValueObjects.Identity;
using DotCruz.Workspaces.Domain.ValueObjects.Workspaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.ProvisionPersonalWorkspace;

public class ProvisionPersonalWorkspaceCommandHandler : IRequestHandler<ProvisionPersonalWorkspaceCommand>
{
    private readonly IWorkspaceWriteRepository _WorkspaceWriteRepository;
    private readonly IWorkspaceReadRepository _WorkspaceReadRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProvisionPersonalWorkspaceCommandHandler> _logger;

    public ProvisionPersonalWorkspaceCommandHandler(
        IWorkspaceWriteRepository WorkspaceWriteRepository,
        IWorkspaceReadRepository WorkspaceReadRepository,
        IUnitOfWork unitOfWork,
        ILogger<ProvisionPersonalWorkspaceCommandHandler> logger)
    {
        _WorkspaceWriteRepository = WorkspaceWriteRepository;
        _WorkspaceReadRepository = WorkspaceReadRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(ProvisionPersonalWorkspaceCommand request, CancellationToken cancellationToken)
    {
        var existingWorkspaces = await _WorkspaceReadRepository.GetUserWorkspacesAsync(request.UserId, cancellationToken);
        if (existingWorkspaces.Any(w => w.Workspace.Type == WorkspaceType.Personal))
        {
            _logger.LogInformation("Personal workspace already exists for user {UserId}. Skipping provisioning.", request.UserId);
            return;
        }

        var slugValue = $"personal-{request.UserId:N}";
        var slug = WorkspaceSlug.Create(slugValue);

        var workspaceName = string.IsNullOrWhiteSpace(request.Name)
            ? "Workspace Pessoal"
            : $"Workspace de {request.Name.Trim()}";

        if (workspaceName.Length > 100)
            workspaceName = workspaceName[..100];

        var name = Name.Create(workspaceName);
        var email = Email.Create(request.Email);

        var workspace = Workspace.CreateWorkspace(
            name: name,
            slug: slug,
            type: WorkspaceType.Personal,
            ownerEmail: email,
            ownerUserId: request.UserId
        );

        await _WorkspaceWriteRepository.AddAsync(workspace);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Personal workspace {WorkspaceId} provisioned successfully for user {UserId}", workspace.Id, request.UserId);
    }
}
