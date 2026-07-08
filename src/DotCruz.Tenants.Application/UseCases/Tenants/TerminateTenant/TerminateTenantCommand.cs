using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.TerminateTenant;

public record TerminateTenantCommand(Guid Id) : IRequest;
