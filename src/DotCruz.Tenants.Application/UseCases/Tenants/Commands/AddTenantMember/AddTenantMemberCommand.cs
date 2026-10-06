using DotCruz.Tenants.Application.Abstractions.Security;
using DotCruz.Tenants.Domain.Enums.Tenants;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.AddTenantMember;

public sealed record AddTenantMemberCommand(
    Guid TenantId,
    string Email,
    TenantRole Role = TenantRole.Member,
    Guid? UserId = null
) : IRequest<Guid>, ITenantScopedRequest;
