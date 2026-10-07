using CommonTestUtilities.Commands.Tenants;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.UpdateWorkspaceSubscription;
using DotCruz.Workspaces.Domain.Enums.Workspaces;
using DotCruz.Workspaces.Domain.Exceptions.Resources;

namespace Validators.Test.Tenants.UpdateWorkspaceSubscription;

public class UpdateWorkspaceSubscriptionCommandValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = UpdateWorkspaceSubscriptionCommandBuilder.Build();

        var result = new UpdateWorkspaceSubscriptionCommandValidator().Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Error_Id_Empty()
    {
        var request = UpdateWorkspaceSubscriptionCommandBuilder.Build(id: Guid.Empty);

        var result = new UpdateWorkspaceSubscriptionCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.ID_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_Plan_Invalid()
    {
        var request = UpdateWorkspaceSubscriptionCommandBuilder.Build(plan: (PlanType)99);

        var result = new UpdateWorkspaceSubscriptionCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.PLAN_INVALID, error.ErrorMessage);
    }
}
