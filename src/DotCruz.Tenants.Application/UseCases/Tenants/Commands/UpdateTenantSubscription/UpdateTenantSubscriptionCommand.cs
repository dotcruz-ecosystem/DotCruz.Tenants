using DotCruz.Tenants.Domain.Enums.Tenants;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.UpdateTenantSubscription;

public sealed record UpdateTenantSubscriptionCommand(
    Guid Id,
    PlanType Plan
) : IRequest;
