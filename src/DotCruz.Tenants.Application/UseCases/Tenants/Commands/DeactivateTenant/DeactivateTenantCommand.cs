using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.DeactivateTenant;

public sealed record DeactivateTenantCommand(Guid Id) : IRequest;
