using DotCruz.Workspaces.Application.DTOs.Workspaces;
using MediatR;
using DotCruz.Workspaces.Application.Abstractions.Security;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetTenantById;

public record GetTenantByIdQuery(Guid Id) : IRequest<WorkspaceDto>, IWorkspaceScopedRequest
{
    Guid IWorkspaceScopedRequest.WorkspaceId => Id;
}
