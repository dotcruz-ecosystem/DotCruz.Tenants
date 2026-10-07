using MediatR;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.ProvisionPersonalWorkspace;

public sealed record ProvisionPersonalWorkspaceCommand(
    Guid UserId,
    string Name,
    string Email
) : IRequest;
