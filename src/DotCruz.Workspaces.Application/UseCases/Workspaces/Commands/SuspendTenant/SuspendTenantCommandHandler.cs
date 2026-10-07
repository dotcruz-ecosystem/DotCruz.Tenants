using DotCruz.Workspaces.Application.Abstractions.Data;
using DotCruz.Workspaces.Application.Abstractions.Data.Repositories.Workspaces;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;
using DotCruz.Workspaces.Domain.ValueObjects.Workspaces;
using MediatR;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.SuspendTenant;

public class SuspendTenantCommandHandler : IRequestHandler<SuspendTenantCommand>
{
    private readonly IWorkspaceWriteRepository _WorkspaceWriteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SuspendTenantCommandHandler(IWorkspaceWriteRepository WorkspaceWriteRepository, IUnitOfWork unitOfWork)
    {
        _WorkspaceWriteRepository = WorkspaceWriteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(SuspendTenantCommand request, CancellationToken cancellationToken)
    {
        var Workspace = await _WorkspaceWriteRepository.GetByIdToUpdateAsync(request.Id, cancellationToken) 
            ?? throw new NotFoundException(ResourceMessagesException.TENANT_NOT_FOUND);
        
        var reason = SuspensionReason.Create(request.Reason);
        
        Workspace.Suspend(reason);

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
