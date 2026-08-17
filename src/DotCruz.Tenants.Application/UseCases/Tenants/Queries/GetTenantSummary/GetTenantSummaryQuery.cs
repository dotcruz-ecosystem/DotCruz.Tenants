using DotCruz.Tenants.Application.DTOs.Tenants;
using MediatR;
using DotCruz.Tenants.Application.Abstractions.Security;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Queries.GetTenantSummary;

public sealed record GetTenantSummaryQuery(Guid Id) : IRequest<TenantSummaryDto>, ITenantScopedRequest
{
    Guid ITenantScopedRequest.TenantId => Id;
}
