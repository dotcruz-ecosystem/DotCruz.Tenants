using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;
using DotCruz.Tenants.Application.DTOs.Tenants;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Queries.GetUserWorkspaces;

public class GetUserWorkspacesQueryHandler : IRequestHandler<GetUserWorkspacesQuery, IReadOnlyCollection<UserWorkspaceDto>>
{
    private readonly ITenantReadRepository _tenantReadRepository;

    public GetUserWorkspacesQueryHandler(ITenantReadRepository tenantReadRepository)
    {
        _tenantReadRepository = tenantReadRepository;
    }

    public async Task<IReadOnlyCollection<UserWorkspaceDto>> Handle(GetUserWorkspacesQuery request, CancellationToken cancellationToken)
    {
        var workspaces = await _tenantReadRepository.GetUserWorkspacesAsync(request.UserId, cancellationToken);

        return workspaces.Select(w => new UserWorkspaceDto(
            w.Tenant.Id,
            w.Tenant.Name.Value,
            w.Tenant.Slug.Value,
            w.Tenant.Type.ToString(),
            w.Tenant.Status.ToString(),
            w.Member.Role.ToString(),
            w.Member.Status.ToString(),
            w.Tenant.Subscription.Plan.ToString()
        )).ToList().AsReadOnly();
    }
}
