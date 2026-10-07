using DotCruz.Workspaces.Application.DTOs.Workspaces;
using MediatR;
using DotCruz.Workspaces.Application.Abstractions.Security;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.ConfigureTenantSmtp;

public sealed record ConfigureTenantSmtpCommand(
    Guid WorkspaceId,
    ConfigureWorkspaceSmtpDto Request
) : IRequest, IWorkspaceScopedRequest;
