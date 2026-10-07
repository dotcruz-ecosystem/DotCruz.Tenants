using DotCruz.Workspaces.Application.Abstractions.Data;
using DotCruz.Workspaces.Application.Abstractions.Data.Repositories.Workspaces;
using DotCruz.Workspaces.Application.Mappers.Workspaces;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;
using MediatR;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.UpdateWorkspaceContact;

public class UpdateWorkspaceContactCommandHandler : IRequestHandler<UpdateWorkspaceContactCommand>
{
    private readonly IWorkspaceWriteRepository _WorkspaceWriteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateWorkspaceContactCommandHandler(
        IWorkspaceWriteRepository WorkspaceWriteRepository,
        IUnitOfWork unitOfWork)
    {
        _WorkspaceWriteRepository = WorkspaceWriteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateWorkspaceContactCommand request, CancellationToken cancellationToken)
    {
        var Workspace = await _WorkspaceWriteRepository.GetByIdToUpdateAsync(request.Id, cancellationToken) 
            ?? throw new NotFoundException(ResourceMessagesException.TENANT_NOT_FOUND);
       
        var newContact = request.WorkspaceContact.ToDomain();
        
        Workspace.UpdateContact(newContact);

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
