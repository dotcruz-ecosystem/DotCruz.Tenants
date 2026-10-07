using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.CreateTenant;
using MediatR;

namespace DotCruz.Workspaces.Application.Events.CreatedTenant;

public sealed record CreatedTenantEvent(Guid WorkspaceId, CreateTenantCommand Request) : INotification;
