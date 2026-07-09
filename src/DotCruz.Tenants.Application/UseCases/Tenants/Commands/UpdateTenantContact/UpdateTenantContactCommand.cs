using DotCruz.Tenants.Application.DTOs.Tenants;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.UpdateTenantContact;

public sealed record UpdateTenantContactCommand(
    Guid Id,
    TenantContactDto TenantContact
) : IRequest;
