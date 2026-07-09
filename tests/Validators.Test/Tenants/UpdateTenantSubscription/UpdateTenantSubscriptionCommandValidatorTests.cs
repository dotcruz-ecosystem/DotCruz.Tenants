using CommonTestUtilities.Commands.Tenants;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.UpdateTenantSubscription;
using DotCruz.Tenants.Domain.Enums.Tenants;
using DotCruz.Tenants.Domain.Exceptions.Resources;

namespace Validators.Test.Tenants.UpdateTenantSubscription;

public class UpdateTenantSubscriptionCommandValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = UpdateTenantSubscriptionCommandBuilder.Build();

        var result = new UpdateTenantSubscriptionCommandValidator().Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Error_Id_Empty()
    {
        var request = UpdateTenantSubscriptionCommandBuilder.Build(id: Guid.Empty);

        var result = new UpdateTenantSubscriptionCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.ID_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_Plan_Invalid()
    {
        var request = UpdateTenantSubscriptionCommandBuilder.Build(plan: (PlanType)99);

        var result = new UpdateTenantSubscriptionCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.PLAN_INVALID, error.ErrorMessage);
    }
}
