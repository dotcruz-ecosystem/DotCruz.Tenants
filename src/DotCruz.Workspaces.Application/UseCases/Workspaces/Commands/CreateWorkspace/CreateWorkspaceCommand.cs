using DotCruz.Workspaces.Application.DTOs.Workspaces;
using DotCruz.Workspaces.Domain.Enums.Workspaces;
using MediatR;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.CreateWorkspace;

public sealed record CreateWorkspaceCommand(
    string Name,
    string Slug,
    WorkspaceType Type = WorkspaceType.Personal,
    string? OwnerEmail = null
) : IRequest<UserWorkspaceDto>;
