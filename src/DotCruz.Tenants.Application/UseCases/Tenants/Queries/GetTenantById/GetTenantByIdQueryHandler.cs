using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;
using DotCruz.Tenants.Application.DTOs.Tenants;
using DotCruz.Tenants.Application.Mappers.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Queries.GetTenantById;

public class GetTenantByIdQueryHandler : IRequestHandler<GetTenantByIdQuery, TenantDto>
{
    private readonly ITenantReadRepository _tenantReadRepository;

    public GetTenantByIdQueryHandler(ITenantReadRepository tenantReadRepository)
    {
        _tenantReadRepository = tenantReadRepository;
    }

    public async Task<TenantDto> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantReadRepository.GetByIdAsync(request.Id, cancellationToken);

        if (tenant is null)
            throw new NotFoundException(ResourceMessagesException.TENANT_NOT_FOUND);

        return tenant.ToDto();
    }
}
