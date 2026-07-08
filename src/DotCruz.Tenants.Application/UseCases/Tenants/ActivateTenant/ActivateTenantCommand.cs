using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.ActivateTenant;

public sealed record ActivateTenantCommand(Guid Id) : IRequest;
