namespace DotCruz.Workspaces.Application.Abstractions.Services.Smtp;

public interface ISmtpConfigService
{
    Task SaveAsync(Guid WorkspaceId, string host, int port, string username, string password, string fromName, CancellationToken cancellationToken);
}
