using DotCruz.Tenants.Application.UseCases.Tenants.Commands.CreateTenant;
using MediatR;

namespace DotCruz.Tenants.Application.Events.CreatedTenant;

public sealed record CreatedTenantEvent(Guid TenantId, CreateTenantCommand Request) : INotification;
