using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.ProvisionPersonalWorkspace;

public sealed record ProvisionPersonalWorkspaceCommand(
    Guid UserId,
    string Name,
    string Email
) : IRequest;
