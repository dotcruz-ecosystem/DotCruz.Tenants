using CommonTestUtilities.Commands.Tenants;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.AddWorkspaceMember;
using DotCruz.Workspaces.Domain.Exceptions.Resources;

namespace Validators.Test.Tenants.AddWorkspaceMember;

public class AddWorkspaceMemberCommandValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = AddWorkspaceMemberCommandBuilder.Build();

        var result = new AddWorkspaceMemberCommandValidator().Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Error_WorkspaceId_Empty()
    {
        var request = AddWorkspaceMemberCommandBuilder.Build(isWorkspaceIdEmpty: true);

        var result = new AddWorkspaceMemberCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.ID_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_Email_Empty()
    {
        var request = AddWorkspaceMemberCommandBuilder.Build(isEmailEmpty: true);

        var result = new AddWorkspaceMemberCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceMessagesException.EMAIL_EMPTY);
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var request = AddWorkspaceMemberCommandBuilder.Build(isEmailInvalid: true);

        var result = new AddWorkspaceMemberCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceMessagesException.EMAIL_INVALID);
    }
}
