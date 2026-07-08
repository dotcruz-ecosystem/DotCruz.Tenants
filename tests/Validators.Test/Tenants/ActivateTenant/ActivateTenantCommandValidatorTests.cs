using CommonTestUtilities.Commands.Tenants;
using DotCruz.Tenants.Application.UseCases.Tenants.ActivateTenant;
using DotCruz.Tenants.Domain.Exceptions.Resources;

namespace Validators.Test.Tenants.ActivateTenant;

public class ActivateTenantCommandValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = ActivateTenantCommandBuilder.Build();

        var result = new ActivateTenantCommandValidator().Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Error_Id_Empty()
    {
        var request = ActivateTenantCommandBuilder.Build(id: Guid.Empty);

        var result = new ActivateTenantCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.ID_EMPTY, error.ErrorMessage);
    }
}
