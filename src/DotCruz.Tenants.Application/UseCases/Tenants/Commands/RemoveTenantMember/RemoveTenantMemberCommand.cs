using DotCruz.Tenants.Application.Abstractions.Security;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.RemoveTenantMember;

public sealed record RemoveTenantMemberCommand(
    Guid TenantId,
    Guid MemberId
) : IRequest, ITenantScopedRequest;
