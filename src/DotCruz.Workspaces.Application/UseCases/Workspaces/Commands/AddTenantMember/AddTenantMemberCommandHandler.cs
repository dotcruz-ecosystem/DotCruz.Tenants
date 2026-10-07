using DotCruz.Workspaces.Application.Abstractions.Data;
using DotCruz.Workspaces.Application.Abstractions.Data.Repositories.Workspaces;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;
using DotCruz.Workspaces.Domain.ValueObjects.Communication;
using MediatR;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.AddWorkspaceMember;

public class AddWorkspaceMemberCommandHandler : IRequestHandler<AddWorkspaceMemberCommand, Guid>
{
    private readonly IWorkspaceWriteRepository _WorkspaceWriteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddWorkspaceMemberCommandHandler(
        IWorkspaceWriteRepository WorkspaceWriteRepository,
        IUnitOfWork unitOfWork)
    {
        _WorkspaceWriteRepository = WorkspaceWriteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(AddWorkspaceMemberCommand request, CancellationToken cancellationToken)
    {
        var Workspace = await _WorkspaceWriteRepository.GetByIdWithMembersToUpdateAsync(request.WorkspaceId, cancellationToken)
            ?? throw new NotFoundException(ResourceMessagesException.TENANT_NOT_FOUND);

        var email = Email.Create(request.Email);
        var member = Workspace.AddMember(email, request.Role, request.UserId);

        await _unitOfWork.CommitAsync(cancellationToken);

        return member.Id;
    }
}
