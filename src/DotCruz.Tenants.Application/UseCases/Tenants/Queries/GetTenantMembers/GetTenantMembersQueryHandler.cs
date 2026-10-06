using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;
using DotCruz.Tenants.Application.DTOs.Tenants;
using DotCruz.Tenants.Application.Mappers.Tenants;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Queries.GetTenantMembers;

public class GetTenantMembersQueryHandler : IRequestHandler<GetTenantMembersQuery, IReadOnlyCollection<TenantMemberDto>>
{
    private readonly ITenantReadRepository _tenantReadRepository;

    public GetTenantMembersQueryHandler(ITenantReadRepository tenantReadRepository)
    {
        _tenantReadRepository = tenantReadRepository;
    }

    public async Task<IReadOnlyCollection<TenantMemberDto>> Handle(GetTenantMembersQuery request, CancellationToken cancellationToken)
    {
        var members = await _tenantReadRepository.GetMembersAsync(request.TenantId, cancellationToken);
        return members.Select(m => m.ToDto()).ToList().AsReadOnly();
    }
}
