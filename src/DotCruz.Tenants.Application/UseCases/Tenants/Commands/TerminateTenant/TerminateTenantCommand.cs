using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.TerminateTenant;

public record TerminateTenantCommand(Guid Id) : IRequest;
