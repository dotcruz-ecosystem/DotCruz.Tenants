using DotCruz.Workspaces.Application.DTOs.Workspaces;
using MediatR;
using DotCruz.Workspaces.Application.Abstractions.Security;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.UpdateWorkspaceAddress;

public sealed record UpdateWorkspaceAddressCommand(
    Guid Id,
    WorkspaceAddressDto WorkspaceAddress
) : IRequest, IWorkspaceScopedRequest
{
    Guid IWorkspaceScopedRequest.WorkspaceId => Id;
}
