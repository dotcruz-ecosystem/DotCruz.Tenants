using DotCruz.Workspaces.Application.Abstractions.Data;
using DotCruz.Workspaces.Application.Abstractions.Data.Repositories.Workspaces;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;
using MediatR;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.DeactivateTenant;

public class DeactivateTenantCommandHandler : IRequestHandler<DeactivateTenantCommand>
{
    private readonly IWorkspaceWriteRepository _WorkspaceWriteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateTenantCommandHandler(IWorkspaceWriteRepository WorkspaceWriteRepository, IUnitOfWork unitOfWork)
    {
        _WorkspaceWriteRepository = WorkspaceWriteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeactivateTenantCommand request, CancellationToken cancellationToken)
    {
        var Workspace = await _WorkspaceWriteRepository.GetByIdToUpdateAsync(request.Id, cancellationToken) 
            ?? throw new NotFoundException(ResourceMessagesException.TENANT_NOT_FOUND);
        
        Workspace.Deactivate();

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
