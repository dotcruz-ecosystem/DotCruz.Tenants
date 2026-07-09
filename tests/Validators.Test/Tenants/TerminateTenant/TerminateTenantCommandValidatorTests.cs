using CommonTestUtilities.Commands.Tenants;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.TerminateTenant;
using DotCruz.Tenants.Domain.Exceptions.Resources;

namespace Validators.Test.Tenants.TerminateTenant;

public class TerminateTenantCommandValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = TerminateTenantCommandBuilder.Build();

        var result = new TerminateTenantCommandValidator().Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Error_Id_Empty()
    {
        var request = TerminateTenantCommandBuilder.Build(id: Guid.Empty);

        var result = new TerminateTenantCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.ID_EMPTY, error.ErrorMessage);
    }
}
