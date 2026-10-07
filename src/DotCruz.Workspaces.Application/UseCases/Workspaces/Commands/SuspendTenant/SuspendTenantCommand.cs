using MediatR;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.SuspendTenant;

public sealed record SuspendTenantCommand(Guid Id, string Reason) : IRequest;
