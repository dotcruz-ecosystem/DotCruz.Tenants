using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.ActivateTenant;

public sealed record ActivateTenantCommand(Guid Id) : IRequest;
