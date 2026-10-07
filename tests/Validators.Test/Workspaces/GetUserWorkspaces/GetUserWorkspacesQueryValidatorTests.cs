using CommonTestUtilities.Queries.Workspaces;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetUserWorkspaces;
using DotCruz.Workspaces.Domain.Exceptions.Resources;

namespace Validators.Test.Tenants.GetUserWorkspaces;

public class GetUserWorkspacesQueryValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = GetUserWorkspacesQueryBuilder.Build();

        var result = new GetUserWorkspacesQueryValidator().Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Error_UserId_Empty()
    {
        var request = GetUserWorkspacesQueryBuilder.Build(isUserIdEmpty: true);

        var result = new GetUserWorkspacesQueryValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.USER_ID_REQUIRED, error.ErrorMessage);
    }
}
