using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.DeactivateTenant;

public sealed record DeactivateTenantCommand(Guid Id) : IRequest;
