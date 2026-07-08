using DotCruz.Tenants.Application.DTOs.Tenants;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.GetTenantById;

public record GetTenantByIdQuery(Guid Id) : IRequest<TenantDto>;
