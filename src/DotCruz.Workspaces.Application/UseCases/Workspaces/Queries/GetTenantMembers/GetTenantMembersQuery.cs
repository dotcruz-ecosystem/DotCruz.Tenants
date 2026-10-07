using DotCruz.Workspaces.Application.Abstractions.Security;
using DotCruz.Workspaces.Application.DTOs.Workspaces;
using MediatR;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetWorkspaceMembers;

public sealed record GetWorkspaceMembersQuery(Guid WorkspaceId) : IRequest<IReadOnlyCollection<WorkspaceMemberDto>>, IWorkspaceScopedRequest;
