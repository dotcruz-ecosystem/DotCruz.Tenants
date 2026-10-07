using DotCruz.Workspaces.Domain.Enums.Workspaces;
using MediatR;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.UpdateWorkspaceSubscription;

public sealed record UpdateWorkspaceSubscriptionCommand(
    Guid Id,
    PlanType Plan
) : IRequest;
