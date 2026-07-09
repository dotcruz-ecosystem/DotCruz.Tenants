using DotCruz.Tenants.Application.Abstractions.Services.CoreAuth;
using DotCruz.Tenants.Application.Abstractions.Services.CoreAuth.Requests;
using MediatR;

namespace DotCruz.Tenants.Application.Events.CreatedTenant.Consumers;

public class CreateTenantAdminUser(ICoreAuthClient client) : INotificationHandler<CreatedTenantEvent>
{
    public async Task Handle(CreatedTenantEvent notification, CancellationToken cancellationToken)
    {
        var request = new CreateTenantAdminUserRequestDto(
            notification.Request.TenantAdminUser.AdminName,
            notification.Request.TenantAdminUser.AdminEmail,
            notification.TenantId
        );

        await client.CreateTenantAdminUser(request, cancellationToken);
    }
}
