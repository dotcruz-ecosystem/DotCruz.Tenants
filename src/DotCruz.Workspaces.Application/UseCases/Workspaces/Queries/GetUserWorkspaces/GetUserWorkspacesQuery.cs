using DotCruz.Workspaces.Application.DTOs.Workspaces;
using MediatR;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetUserWorkspaces;

public sealed record GetUserWorkspacesQuery(Guid UserId) : IRequest<IReadOnlyCollection<UserWorkspaceDto>>;
