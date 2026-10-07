using DotCruz.Shared.Security.Context;
using DotCruz.Workspaces.Application.Abstractions.Data;
using DotCruz.Workspaces.Application.Abstractions.Data.Repositories.Workspaces;
using DotCruz.Workspaces.Application.Mappers.Workspaces;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;
using MediatR;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.UpdateWorkspaceBranding;

public class UpdateWorkspaceBrandingCommandHandler : IRequestHandler<UpdateWorkspaceBrandingCommand>
{
    private readonly IWorkspaceWriteRepository _WorkspaceWriteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISecurityContext _securityContext;

    public UpdateWorkspaceBrandingCommandHandler(
        IWorkspaceWriteRepository WorkspaceWriteRepository,
        IUnitOfWork unitOfWork,
        ISecurityContext securityContext
    )
    {
        _WorkspaceWriteRepository = WorkspaceWriteRepository;
        _unitOfWork = unitOfWork;
        _securityContext = securityContext;
    }

    public async Task Handle(UpdateWorkspaceBrandingCommand request, CancellationToken cancellationToken)
    {
        if (_securityContext.WorkspaceId != request.Id)
            throw new ForbiddenException(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE);

        var Workspace = await _WorkspaceWriteRepository.GetByIdToUpdateAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(ResourceMessagesException.TENANT_NOT_FOUND);

        var newBranding = request.WorkspaceBranding.ToDomain();

        Workspace.UpdateBranding(newBranding);

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
