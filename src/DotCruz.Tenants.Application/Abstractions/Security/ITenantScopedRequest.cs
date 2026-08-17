namespace DotCruz.Tenants.Application.Abstractions.Security;

public interface ITenantScopedRequest
{
    Guid TenantId { get; }
}
