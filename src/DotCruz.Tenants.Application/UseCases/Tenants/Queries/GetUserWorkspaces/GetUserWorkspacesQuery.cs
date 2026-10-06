using DotCruz.Tenants.Application.DTOs.Tenants;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Queries.GetUserWorkspaces;

public sealed record GetUserWorkspacesQuery(Guid UserId) : IRequest<IReadOnlyCollection<UserWorkspaceDto>>;
