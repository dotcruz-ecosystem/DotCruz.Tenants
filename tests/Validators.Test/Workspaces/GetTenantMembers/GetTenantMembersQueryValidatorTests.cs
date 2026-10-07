using CommonTestUtilities.Queries.Workspaces;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetWorkspaceMembers;
using DotCruz.Workspaces.Domain.Exceptions.Resources;

namespace Validators.Test.Tenants.GetWorkspaceMembers;

public class GetWorkspaceMembersQueryValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = GetWorkspaceMembersQueryBuilder.Build();

        var result = new GetWorkspaceMembersQueryValidator().Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Error_WorkspaceId_Empty()
    {
        var request = GetWorkspaceMembersQueryBuilder.Build(isWorkspaceIdEmpty: true);

        var result = new GetWorkspaceMembersQueryValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.ID_EMPTY, error.ErrorMessage);
    }
}
