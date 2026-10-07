using DotCruz.Workspaces.Application.DTOs.Workspaces;
using MediatR;
using DotCruz.Workspaces.Application.Abstractions.Security;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.UpdateWorkspaceContact;

public sealed record UpdateWorkspaceContactCommand(
    Guid Id,
    WorkspaceContactDto WorkspaceContact
) : IRequest, IWorkspaceScopedRequest
{
    Guid IWorkspaceScopedRequest.WorkspaceId => Id;
}
