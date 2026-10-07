using DotCruz.Workspaces.Application.Abstractions.Security;
using MediatR;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.RemoveWorkspaceMember;

public sealed record RemoveWorkspaceMemberCommand(
    Guid WorkspaceId,
    Guid MemberId
) : IRequest, IWorkspaceScopedRequest;
