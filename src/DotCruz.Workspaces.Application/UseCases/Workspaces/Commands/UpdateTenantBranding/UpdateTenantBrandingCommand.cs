using DotCruz.Workspaces.Application.DTOs.Workspaces;
using MediatR;
using DotCruz.Workspaces.Application.Abstractions.Security;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.UpdateWorkspaceBranding;

public sealed record UpdateWorkspaceBrandingCommand(
    Guid Id,
    WorkspaceBrandingDto WorkspaceBranding
) : IRequest, IWorkspaceScopedRequest
{
    Guid IWorkspaceScopedRequest.WorkspaceId => Id;
}
