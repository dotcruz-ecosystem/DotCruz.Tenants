using MediatR;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.DeactivateTenant;

public sealed record DeactivateTenantCommand(Guid Id) : IRequest;
