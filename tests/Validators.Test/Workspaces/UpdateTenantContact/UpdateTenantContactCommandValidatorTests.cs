using CommonTestUtilities.Commands.Tenants;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.UpdateWorkspaceContact;
using DotCruz.Workspaces.Domain.Exceptions.Resources;

namespace Validators.Test.Tenants.UpdateWorkspaceContact;

public class UpdateWorkspaceContactCommandValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = UpdateWorkspaceContactCommandBuilder.Build();

        var result = new UpdateWorkspaceContactCommandValidator().Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Error_Id_Empty()
    {
        var request = UpdateWorkspaceContactCommandBuilder.Build(id: Guid.Empty);

        var result = new UpdateWorkspaceContactCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.ID_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_Email_Empty()
    {
        var request = UpdateWorkspaceContactCommandBuilder.Build(isEmailEmpty: true);

        var result = new UpdateWorkspaceContactCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.EMAIL_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_PhoneCountryCode_Invalid()
    {
        var request = UpdateWorkspaceContactCommandBuilder.Build(isCountryCodeInvalid: true);

        var result = new UpdateWorkspaceContactCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.PHONE_COUNTRY_CODE_INVALID, error.ErrorMessage);
    }

    [Fact]
    public void Error_PhoneNationalNumber_Empty()
    {
        var request = UpdateWorkspaceContactCommandBuilder.Build(isPhoneEmpty: true);

        var result = new UpdateWorkspaceContactCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.PHONE_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_WorkspaceContact_Null()
    {
        var request = UpdateWorkspaceContactCommandBuilder.Build(isWorkspaceContactNull: true);

        var result = new UpdateWorkspaceContactCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.CONTACT_REQUIRED, error.ErrorMessage);
    }
}
