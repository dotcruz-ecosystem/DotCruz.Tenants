using DotCruz.Tenants.Application.DTOs.Tenants;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.UpdateTenantAddress;

public sealed record UpdateTenantAddressCommand(
    Guid Id,
    TenantAddressDto TenantAddress
) : IRequest;
