using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;
using DotCruz.Tenants.Application.DTOs.Tenants;
using DotCruz.Tenants.Application.Mappers.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Queries.GetTenantSummary;

public class GetTenantSummaryQueryHandler : IRequestHandler<GetTenantSummaryQuery, TenantSummaryDto>
{
    private readonly ITenantReadRepository _tenantReadRepository;

    public GetTenantSummaryQueryHandler(ITenantReadRepository tenantReadRepository)
    {
        _tenantReadRepository = tenantReadRepository;
    }

    public async Task<TenantSummaryDto> Handle(GetTenantSummaryQuery request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantReadRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(ResourceMessagesException.TENANT_NOT_FOUND);

        return tenant.ToSummaryDto();
    }
}
