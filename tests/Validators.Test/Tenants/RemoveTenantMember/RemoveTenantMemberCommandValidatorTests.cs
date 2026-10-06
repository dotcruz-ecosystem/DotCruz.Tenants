using CommonTestUtilities.Commands.Tenants;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.RemoveTenantMember;
using DotCruz.Tenants.Domain.Exceptions.Resources;

namespace Validators.Test.Tenants.RemoveTenantMember;

public class RemoveTenantMemberCommandValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = RemoveTenantMemberCommandBuilder.Build();

        var result = new RemoveTenantMemberCommandValidator().Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Error_TenantId_Empty()
    {
        var request = RemoveTenantMemberCommandBuilder.Build(isTenantIdEmpty: true);

        var result = new RemoveTenantMemberCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceMessagesException.ID_EMPTY);
    }

    [Fact]
    public void Error_MemberId_Empty()
    {
        var request = RemoveTenantMemberCommandBuilder.Build(isMemberIdEmpty: true);

        var result = new RemoveTenantMemberCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceMessagesException.ID_EMPTY);
    }
}
