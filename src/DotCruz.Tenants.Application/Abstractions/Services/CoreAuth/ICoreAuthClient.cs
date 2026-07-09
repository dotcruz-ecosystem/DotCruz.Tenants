using DotCruz.Tenants.Application.Abstractions.Services.CoreAuth.Requests;

namespace DotCruz.Tenants.Application.Abstractions.Services.CoreAuth;

public interface ICoreAuthClient
{
    Task CreateTenantAdminUser(CreateTenantAdminUserRequestDto requestDto, CancellationToken cancellationToken);
}
