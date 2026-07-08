using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;
using Moq;

namespace CommonTestUtilities.Data.Repositories.Tenants;

public class TenantWriteRepositoryBuilder
{
    public static ITenantWriteRepository Build()
    {
        var tenantWriteRepositoryMock = new Mock<ITenantWriteRepository>();

        return tenantWriteRepositoryMock.Object;
    }
}
