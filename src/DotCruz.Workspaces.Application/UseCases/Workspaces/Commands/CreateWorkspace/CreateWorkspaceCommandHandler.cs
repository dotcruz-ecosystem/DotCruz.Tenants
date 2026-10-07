using DotCruz.Shared.Security.Context;
using DotCruz.Workspaces.Application.Abstractions.Data;
using DotCruz.Workspaces.Application.Abstractions.Data.Repositories.Workspaces;
using DotCruz.Workspaces.Application.DTOs.Workspaces;
using DotCruz.Workspaces.Domain.Entities.Workspaces;
using DotCruz.Workspaces.Domain.Enums.Workspaces;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;
using DotCruz.Workspaces.Domain.ValueObjects.Communication;
using DotCruz.Workspaces.Domain.ValueObjects.Identity;
using DotCruz.Workspaces.Domain.ValueObjects.Workspaces;
using MediatR;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.CreateWorkspace;

public class CreateWorkspaceCommandHandler : IRequestHandler<CreateWorkspaceCommand, UserWorkspaceDto>
{
    private readonly IWorkspaceWriteRepository _WorkspaceWriteRepository;
    private readonly IWorkspaceReadRepository _WorkspaceReadRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISecurityContext _securityContext;

    public CreateWorkspaceCommandHandler(
        IWorkspaceWriteRepository WorkspaceWriteRepository,
        IWorkspaceReadRepository WorkspaceReadRepository,
        IUnitOfWork unitOfWork,
        ISecurityContext securityContext)
    {
        _WorkspaceWriteRepository = WorkspaceWriteRepository;
        _WorkspaceReadRepository = WorkspaceReadRepository;
        _unitOfWork = unitOfWork;
        _securityContext = securityContext;
    }

    public async Task<UserWorkspaceDto> Handle(CreateWorkspaceCommand request, CancellationToken cancellationToken)
    {
        if (!_securityContext.UserId.HasValue)
            throw new ForbiddenException(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE);

        if (await _WorkspaceReadRepository.ExistsWithSlugAsync(request.Slug, cancellationToken))
            throw new ErrorOnValidationException(ResourceMessagesException.SLUG_ALREADY_EXISTS);

        var ownerEmail = !string.IsNullOrWhiteSpace(request.OwnerEmail)
            ? Email.Create(request.OwnerEmail)
            : Email.Create($"user_{_securityContext.UserId.Value:N}@dotcruz.com");

        var workspace = Workspace.CreateWorkspace(
            Name.Create(request.Name),
            WorkspaceSlug.Create(request.Slug),
            request.Type,
            ownerEmail: ownerEmail,
            ownerUserId: _securityContext.UserId.Value
        );

        await _WorkspaceWriteRepository.AddAsync(workspace);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new UserWorkspaceDto(
            workspace.Id,
            workspace.Name.Value,
            workspace.Slug.Value,
            workspace.Type.ToString(),
            workspace.Status.ToString(),
            WorkspaceRole.Owner.ToString(),
            MemberStatus.Active.ToString(),
            workspace.Subscription.Plan.ToString()
        );
    }
}
