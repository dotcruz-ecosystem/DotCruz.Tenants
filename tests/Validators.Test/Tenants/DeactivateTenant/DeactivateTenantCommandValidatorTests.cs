using CommonTestUtilities.Commands.Tenants;
using DotCruz.Tenants.Application.UseCases.Tenants.DeactivateTenant;
using DotCruz.Tenants.Domain.Exceptions.Resources;

namespace Validators.Test.Tenants.DeactivateTenant;

public class DeactivateTenantCommandValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = DeactivateTenantCommandBuilder.Build();

        var result = new DeactivateTenantCommandValidator().Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Error_Id_Empty()
    {
        var request = DeactivateTenantCommandBuilder.Build(id: Guid.Empty);

        var result = new DeactivateTenantCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.ID_EMPTY, error.ErrorMessage);
    }
}
