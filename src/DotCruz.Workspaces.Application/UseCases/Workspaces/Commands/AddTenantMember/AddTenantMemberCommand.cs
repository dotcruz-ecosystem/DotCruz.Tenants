using DotCruz.Workspaces.Application.Abstractions.Security;
using DotCruz.Workspaces.Domain.Enums.Workspaces;
using MediatR;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.AddWorkspaceMember;

public sealed record AddWorkspaceMemberCommand(
    Guid WorkspaceId,
    string Email,
    WorkspaceRole Role = WorkspaceRole.Member,
    Guid? UserId = null
) : IRequest<Guid>, IWorkspaceScopedRequest;
