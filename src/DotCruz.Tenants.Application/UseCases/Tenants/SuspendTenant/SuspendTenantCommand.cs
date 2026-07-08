using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.SuspendTenant;

public sealed record SuspendTenantCommand(Guid Id, string Reason) : IRequest;
