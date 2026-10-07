using MediatR;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.TerminateTenant;

public record TerminateTenantCommand(Guid Id) : IRequest;
