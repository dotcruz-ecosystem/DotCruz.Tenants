using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;
using DotCruz.Tenants.Application.DTOs.Tenants;
using DotCruz.Tenants.Application.Mappers.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.GetTenantBySlug;

public class GetTenantBySlugQueryHandler : IRequestHandler<GetTenantBySlugQuery, TenantDto>
{
    private readonly ITenantReadRepository _tenantReadRepository;

    public GetTenantBySlugQueryHandler(ITenantReadRepository tenantReadRepository)
    {
        _tenantReadRepository = tenantReadRepository;
    }

    public async Task<TenantDto> Handle(GetTenantBySlugQuery request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantReadRepository.GetBySlugAsync(request.Slug, cancellationToken);

        if (tenant is null)
            throw new NotFoundException(ResourceMessagesException.TENANT_NOT_FOUND);

        return tenant.ToDto();
    }
}
