namespace DotCruz.Workspaces.Application.DTOs.Workspaces;

public sealed record ConfigureWorkspaceSmtpDto(
    string Host,
    int Port,
    string Username,
    string Password,
    string FromName
);
