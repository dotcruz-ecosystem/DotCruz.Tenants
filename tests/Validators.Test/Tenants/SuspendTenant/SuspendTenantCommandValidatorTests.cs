using CommonTestUtilities.Commands.Tenants;
using DotCruz.Tenants.Application.UseCases.Tenants.SuspendTenant;
using DotCruz.Tenants.Domain.Exceptions.Resources;

namespace Validators.Test.Tenants.SuspendTenant;

public class SuspendTenantCommandValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = SuspendTenantCommandBuilder.Build();

        var result = new SuspendTenantCommandValidator().Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Error_Id_Empty()
    {
        var request = SuspendTenantCommandBuilder.Build(id: Guid.Empty);

        var result = new SuspendTenantCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceMessagesException.ID_EMPTY);
    }

    [Fact]
    public void Error_Reason_Empty()
    {
        var request = SuspendTenantCommandBuilder.Build(isReasonEmpty: true);

        var result = new SuspendTenantCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.SUSPENSION_REASON_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_Reason_Too_Short()
    {
        var request = SuspendTenantCommandBuilder.Build(isReasonTooShort: true);

        var result = new SuspendTenantCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.SUSPENSION_REASON_INVALID, error.ErrorMessage);
    }

    [Fact]
    public void Error_Reason_Too_Long()
    {
        var request = SuspendTenantCommandBuilder.Build(isReasonTooLong: true);

        var result = new SuspendTenantCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.SUSPENSION_REASON_INVALID, error.ErrorMessage);
    }
}
