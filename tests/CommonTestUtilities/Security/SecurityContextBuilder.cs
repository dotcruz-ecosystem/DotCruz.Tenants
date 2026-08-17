using DotCruz.Shared.Security.Context;
using Moq;

namespace CommonTestUtilities.Security;

public class SecurityContextBuilder
{
    private readonly Mock<ISecurityContext> _securityContext;

    public SecurityContextBuilder()
    {
        _securityContext = new Mock<ISecurityContext>();
        _securityContext.Setup(c => c.Roles).Returns([]);
    }

    public SecurityContextBuilder AsService(string serviceName = "test-service")
    {
        _securityContext.Setup(c => c.IsAuthenticatedService).Returns(true);
        _securityContext.Setup(c => c.IsAuthenticatedUser).Returns(false);
        _securityContext.Setup(c => c.ServiceName).Returns(serviceName);
        _securityContext.Setup(c => c.TenantId).Returns((Guid?)null);
        _securityContext.Setup(c => c.Roles).Returns([]);

        return this;
    }

    public SecurityContextBuilder AsSuperAdmin(Guid? tenantId = null)
    {
        _securityContext.Setup(c => c.IsAuthenticatedService).Returns(false);
        _securityContext.Setup(c => c.IsAuthenticatedUser).Returns(true);
        _securityContext.Setup(c => c.UserId).Returns(Guid.NewGuid());
        _securityContext.Setup(c => c.TenantId).Returns(tenantId);
        _securityContext.Setup(c => c.Roles).Returns(["SuperAdmin"]);

        return this;
    }

    public SecurityContextBuilder AsTenantAdmin(Guid? tenantId = null)
    {
        _securityContext.Setup(c => c.IsAuthenticatedService).Returns(false);
        _securityContext.Setup(c => c.IsAuthenticatedUser).Returns(true);
        _securityContext.Setup(c => c.UserId).Returns(Guid.NewGuid());
        _securityContext.Setup(c => c.TenantId).Returns(tenantId ?? Guid.NewGuid());
        _securityContext.Setup(c => c.Roles).Returns(["TenantAdmin"]);

        return this;
    }

    public ISecurityContext Build() => _securityContext.Object;
}
