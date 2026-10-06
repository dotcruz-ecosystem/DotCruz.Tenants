using CommonTestUtilities.Commands.Tenants;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.AddTenantMember;
using DotCruz.Tenants.Domain.Exceptions.Resources;

namespace Validators.Test.Tenants.AddTenantMember;

public class AddTenantMemberCommandValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = AddTenantMemberCommandBuilder.Build();

        var result = new AddTenantMemberCommandValidator().Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Error_TenantId_Empty()
    {
        var request = AddTenantMemberCommandBuilder.Build(isTenantIdEmpty: true);

        var result = new AddTenantMemberCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.ID_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_Email_Empty()
    {
        var request = AddTenantMemberCommandBuilder.Build(isEmailEmpty: true);

        var result = new AddTenantMemberCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceMessagesException.EMAIL_EMPTY);
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var request = AddTenantMemberCommandBuilder.Build(isEmailInvalid: true);

        var result = new AddTenantMemberCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceMessagesException.EMAIL_INVALID);
    }
}
