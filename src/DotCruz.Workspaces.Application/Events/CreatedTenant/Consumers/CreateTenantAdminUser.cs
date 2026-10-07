using DotCruz.Workspaces.Application.Abstractions.Services.CoreAuth;
using DotCruz.Workspaces.Application.Abstractions.Services.CoreAuth.Requests;
using MediatR;

namespace DotCruz.Workspaces.Application.Events.CreatedTenant.Consumers;

public class CreateTenantAdminUser(ICoreAuthClient client) : INotificationHandler<CreatedTenantEvent>
{
    public async Task Handle(CreatedTenantEvent notification, CancellationToken cancellationToken)
    {
        var request = new CreateWorkspaceAdminUserRequestDto(
            notification.Request.TenantAdminUser.AdminName,
            notification.Request.TenantAdminUser.AdminEmail,
            notification.WorkspaceId
        );

        await client.CreateWorkspaceAdminUser(request, cancellationToken);
    }
}
