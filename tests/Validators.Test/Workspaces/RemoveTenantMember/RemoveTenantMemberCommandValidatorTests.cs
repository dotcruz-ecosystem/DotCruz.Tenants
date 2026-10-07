using CommonTestUtilities.Commands.Tenants;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.RemoveWorkspaceMember;
using DotCruz.Workspaces.Domain.Exceptions.Resources;

namespace Validators.Test.Tenants.RemoveWorkspaceMember;

public class RemoveWorkspaceMemberCommandValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = RemoveWorkspaceMemberCommandBuilder.Build();

        var result = new RemoveWorkspaceMemberCommandValidator().Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Error_WorkspaceId_Empty()
    {
        var request = RemoveWorkspaceMemberCommandBuilder.Build(isWorkspaceIdEmpty: true);

        var result = new RemoveWorkspaceMemberCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceMessagesException.ID_EMPTY);
    }

    [Fact]
    public void Error_MemberId_Empty()
    {
        var request = RemoveWorkspaceMemberCommandBuilder.Build(isMemberIdEmpty: true);

        var result = new RemoveWorkspaceMemberCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceMessagesException.ID_EMPTY);
    }
}
