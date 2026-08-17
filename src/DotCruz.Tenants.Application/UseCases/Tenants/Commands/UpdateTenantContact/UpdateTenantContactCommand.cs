using DotCruz.Tenants.Application.DTOs.Tenants;
using MediatR;
using DotCruz.Tenants.Application.Abstractions.Security;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.UpdateTenantContact;

public sealed record UpdateTenantContactCommand(
    Guid Id,
    TenantContactDto TenantContact
) : IRequest, ITenantScopedRequest
{
    Guid ITenantScopedRequest.TenantId => Id;
}
