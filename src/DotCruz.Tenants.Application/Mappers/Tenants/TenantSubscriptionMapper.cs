using DotCruz.Tenants.Application.DTOs.Tenants;
using DotCruz.Tenants.Domain.ValueObjects.Temporal;
using DotCruz.Tenants.Domain.ValueObjects.Tenants;

namespace DotCruz.Tenants.Application.Mappers.Tenants;

public static class TenantSubscriptionMapper
{
    public static TenantSubscriptionDto ToDto(this TenantSubscription subscription)
    {
        return new TenantSubscriptionDto(
            subscription.Plan,
            subscription.StartDate,
            subscription.EndDate,
            subscription.TrialEndDate,
            subscription.Limits.MaxUsers,
            subscription.Limits.MaxEmailsPerMonth
        );
    }

    public static TenantSubscription ToDomain(this TenantSubscriptionDto subscriptionDto)
    {
        var duration = new DateTimePeriod(subscriptionDto.StartDate, subscriptionDto.EndDate);
        var limits = new ResourceLimits(subscriptionDto.MaxUsers, subscriptionDto.MaxEmailsPerMonth);
        return new TenantSubscription(
            subscriptionDto.Plan,
            duration,
            subscriptionDto.TrialEndDate,
            limits
        );
    }
}
