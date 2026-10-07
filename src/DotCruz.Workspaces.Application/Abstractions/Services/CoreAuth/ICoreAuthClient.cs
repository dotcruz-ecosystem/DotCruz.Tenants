using DotCruz.Workspaces.Application.Abstractions.Services.CoreAuth.Requests;

namespace DotCruz.Workspaces.Application.Abstractions.Services.CoreAuth;

public interface ICoreAuthClient
{
    Task CreateWorkspaceAdminUser(CreateWorkspaceAdminUserRequestDto requestDto, CancellationToken cancellationToken);
}
