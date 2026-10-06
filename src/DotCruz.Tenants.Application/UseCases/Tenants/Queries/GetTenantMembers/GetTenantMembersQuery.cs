using DotCruz.Tenants.Application.Abstractions.Security;
using DotCruz.Tenants.Application.DTOs.Tenants;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Queries.GetTenantMembers;

public sealed record GetTenantMembersQuery(Guid TenantId) : IRequest<IReadOnlyCollection<TenantMemberDto>>, ITenantScopedRequest;
