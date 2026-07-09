using DotCruz.Tenants.Application.DTOs.Tenants;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Queries.GetTenantBySlug;

public record GetTenantBySlugQuery(string Slug) : IRequest<TenantDto>;
