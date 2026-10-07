using DotCruz.Workspaces.Application.DTOs.Workspaces;
using MediatR;
using DotCruz.Workspaces.Application.Abstractions.Security;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetTenantSummary;

public sealed record GetTenantSummaryQuery(Guid Id) : IRequest<WorkspaceSummaryDto>, IWorkspaceScopedRequest
{
    Guid IWorkspaceScopedRequest.WorkspaceId => Id;
}
