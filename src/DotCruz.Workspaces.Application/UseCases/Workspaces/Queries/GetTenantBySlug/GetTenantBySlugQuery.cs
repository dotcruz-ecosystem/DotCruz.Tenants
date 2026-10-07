using DotCruz.Workspaces.Application.DTOs.Workspaces;
using MediatR;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetTenantBySlug;

public record GetTenantBySlugQuery(string Slug) : IRequest<WorkspaceDto>;
