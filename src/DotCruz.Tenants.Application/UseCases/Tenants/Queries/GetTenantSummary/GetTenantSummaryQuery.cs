using DotCruz.Tenants.Application.DTOs.Tenants;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Queries.GetTenantSummary;

public sealed record GetTenantSummaryQuery(Guid Id) : IRequest<TenantSummaryDto>;
