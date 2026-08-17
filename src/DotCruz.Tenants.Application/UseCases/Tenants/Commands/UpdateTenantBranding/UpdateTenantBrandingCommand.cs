using DotCruz.Tenants.Application.DTOs.Tenants;
using MediatR;
using DotCruz.Tenants.Application.Abstractions.Security;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.UpdateTenantBranding;

public sealed record UpdateTenantBrandingCommand(
    Guid Id,
    TenantBrandingDto TenantBranding
) : IRequest, ITenantScopedRequest
{
    Guid ITenantScopedRequest.TenantId => Id;
}
