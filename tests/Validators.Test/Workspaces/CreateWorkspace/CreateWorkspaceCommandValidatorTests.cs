using CommonTestUtilities.Commands.Tenants;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.CreateWorkspace;
using DotCruz.Workspaces.Domain.Exceptions.Resources;

namespace Validators.Test.Tenants.CreateWorkspace;

public class CreateWorkspaceCommandValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = CreateWorkspaceCommandBuilder.Build();

        var result = new CreateWorkspaceCommandValidator().Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Error_Name_Empty()
    {
        var request = CreateWorkspaceCommandBuilder.Build(isNameEmpty: true);

        var result = new CreateWorkspaceCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.NAME_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_Name_Invalid_Length()
    {
        var request = CreateWorkspaceCommandBuilder.Build(isNameInvalid: true);

        var result = new CreateWorkspaceCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.NAME_INVALID_LENGTH, error.ErrorMessage);
    }

    [Fact]
    public void Error_Slug_Empty()
    {
        var request = CreateWorkspaceCommandBuilder.Build(isSlugEmpty: true);

        var result = new CreateWorkspaceCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.SLUG_EMPTY, error.ErrorMessage);
    }

    [Fact]
    public void Error_Slug_Invalid_Length()
    {
        var request = CreateWorkspaceCommandBuilder.Build(isSlugInvalid: true);

        var result = new CreateWorkspaceCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.SLUG_INVALID_LENGTH, error.ErrorMessage);
    }

    [Fact]
    public void Error_Slug_Reserved()
    {
        var request = CreateWorkspaceCommandBuilder.Build(isSlugReserved: true);

        var result = new CreateWorkspaceCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.SLUG_RESERVED, error.ErrorMessage);
    }

    [Fact]
    public void Error_OwnerEmail_Invalid()
    {
        var request = CreateWorkspaceCommandBuilder.Build(isOwnerEmailInvalid: true);

        var result = new CreateWorkspaceCommandValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.EMAIL_INVALID, error.ErrorMessage);
    }
}
