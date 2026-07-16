using DotCruz.Tenants.Application.DTOs.Tenants;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.UpdateTenantBranding;

public sealed record UpdateTenantBrandingCommand(
    Guid Id,
    TenantBrandingDto TenantBranding
) : IRequest;
