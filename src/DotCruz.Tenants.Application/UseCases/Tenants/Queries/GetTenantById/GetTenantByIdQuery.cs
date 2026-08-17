using DotCruz.Tenants.Application.DTOs.Tenants;
using MediatR;
using DotCruz.Tenants.Application.Abstractions.Security;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Queries.GetTenantById;

public record GetTenantByIdQuery(Guid Id) : IRequest<TenantDto>, ITenantScopedRequest
{
    Guid ITenantScopedRequest.TenantId => Id;
}
