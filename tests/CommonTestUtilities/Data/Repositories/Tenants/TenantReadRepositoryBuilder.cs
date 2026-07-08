using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;
using Moq;

namespace CommonTestUtilities.Data.Repositories.Tenants;

public class TenantReadRepositoryBuilder
{
    private readonly Mock<ITenantReadRepository> _tenantReadRepositoryMock = new();
    
    public void ExistsWithSlugAsync(string slug, CancellationToken cancellationToken)
    {
        _tenantReadRepositoryMock
            .Setup(r => r.ExistsWithSlugAsync(slug, cancellationToken))
            .ReturnsAsync(true);
    }

    public void ExistsWithDocumentAsync(string documentNumber, CancellationToken cancellationToken)
    {
        _tenantReadRepositoryMock
            .Setup(r => r.ExistsWithDocumentAsync(documentNumber, cancellationToken))
            .ReturnsAsync(true);
    }

    public ITenantReadRepository Build() => _tenantReadRepositoryMock.Object;
}
