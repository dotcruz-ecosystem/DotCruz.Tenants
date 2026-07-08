using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;
using DotCruz.Tenants.Domain.Entities.Tenants;
using Moq;

namespace CommonTestUtilities.Data.Repositories.Tenants;

public class TenantWriteRepositoryBuilder
{
    private readonly Mock<ITenantWriteRepository> _repository = new();

    public void GetByIdToUpdate(Tenant tenant, CancellationToken cancellationToken)
    {
        _repository.Setup(x => x.GetByIdToUpdateAsync(tenant.Id, cancellationToken))
            .ReturnsAsync(tenant);
    }

    public ITenantWriteRepository Build() => _repository.Object;
}
