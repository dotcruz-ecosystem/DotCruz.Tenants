using MediatR;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.ActivateTenant;

public sealed record ActivateTenantCommand(Guid Id) : IRequest;
