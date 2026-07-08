using Bogus;
using DotCruz.Tenants.Application.DTOs.Tenants;
using DotCruz.Tenants.Domain.Enums.Tenants;

namespace CommonTestUtilities.DTOs.Tenants;

public class TenantSubscriptionDtoBuilder
{
    public static TenantSubscriptionDto Build(
        PlanType? plan = null,
        bool isTrialEndDateRequiredButNull = false,
        bool isEndDateBeforeStartDate = false,
        bool isMaxUsersInvalid = false,
        bool isMaxEmailsInvalid = false
    )
    {
        var faker = new Faker("pt_BR");
        
        var selectedPlan = plan ?? faker.PickRandom<PlanType>();
        var startDate = DateTimeOffset.UtcNow;
        var endDate = isEndDateBeforeStartDate ? startDate.AddDays(-10) : startDate.AddDays(30);
        
        DateTimeOffset? trialEndDate = null;
        if (selectedPlan == PlanType.Trial)
        {
            trialEndDate = isTrialEndDateRequiredButNull ? null : startDate.AddDays(14);
        }

        var maxUsers = isMaxUsersInvalid ? 0 : faker.Random.Int(min: 1, max: 100);
        var maxEmails = isMaxEmailsInvalid ? 0 : faker.Random.Int(min: 1, max: 10000);

        return new TenantSubscriptionDto(
            Plan: selectedPlan,
            StartDate: startDate,
            EndDate: endDate,
            TrialEndDate: trialEndDate,
            MaxUsers: maxUsers,
            MaxEmailsPerMonth: maxEmails
        );
    }
}
