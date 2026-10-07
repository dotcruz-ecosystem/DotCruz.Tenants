using DotCruz.Workspaces.Application.Abstractions.Data;
using DotCruz.Workspaces.Application.Abstractions.Data.Repositories.Workspaces;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;
using MediatR;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.UpdateWorkspaceSubscription;

public class UpdateWorkspaceSubscriptionCommandHandler : IRequestHandler<UpdateWorkspaceSubscriptionCommand>
{
    private readonly IWorkspaceWriteRepository _WorkspaceWriteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateWorkspaceSubscriptionCommandHandler(
        IWorkspaceWriteRepository WorkspaceWriteRepository,
        IUnitOfWork unitOfWork)
    {
        _WorkspaceWriteRepository = WorkspaceWriteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateWorkspaceSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var Workspace = await _WorkspaceWriteRepository.GetByIdToUpdateAsync(request.Id, cancellationToken) 
            ?? throw new NotFoundException(ResourceMessagesException.TENANT_NOT_FOUND);
        
        var upgradedSubscription = Workspace.Subscription.Upgrade(request.Plan);
        
        Workspace.UpdateSubscription(upgradedSubscription);

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
