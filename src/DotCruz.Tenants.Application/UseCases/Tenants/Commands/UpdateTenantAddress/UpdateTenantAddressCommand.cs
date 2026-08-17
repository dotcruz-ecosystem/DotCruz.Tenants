using DotCruz.Tenants.Application.DTOs.Tenants;
using MediatR;
using DotCruz.Tenants.Application.Abstractions.Security;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.UpdateTenantAddress;

public sealed record UpdateTenantAddressCommand(
    Guid Id,
    TenantAddressDto TenantAddress
) : IRequest, ITenantScopedRequest
{
    Guid ITenantScopedRequest.TenantId => Id;
}
