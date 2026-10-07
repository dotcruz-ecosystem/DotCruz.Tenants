using CommonTestUtilities.ValueObjects.Workspaces;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;
using DotCruz.Workspaces.Domain.ValueObjects.Workspaces;

namespace Domain.Test.ValueObjects.Workspaces;

public class WorkspaceSlugTests
{
    [Fact]
    public void Success()
    {
        var slug = WorkspaceSlugBuilder.Build();

        Assert.NotNull(slug);
        Assert.NotEmpty(slug.Value);
        Assert.Equal(slug.Value, slug.ToString());
    }

    [Fact]
    public void Success_Casing_Normalized()
    {
        var slug = WorkspaceSlug.Create("My-Awesome-Slug-123");

        Assert.Equal("my-awesome-slug-123", slug.Value);
    }

    [Fact]
    public void Error_Slug_Empty()
    {
        static WorkspaceSlug act() => WorkspaceSlugBuilder.Build(isEmpty: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.SLUG_EMPTY, exceptionMessage);
    }

    [Fact]
    public void Error_Slug_TooShort()
    {
        static WorkspaceSlug act() => WorkspaceSlugBuilder.Build(isTooShort: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.SLUG_INVALID_LENGTH, exceptionMessage);
    }

    [Fact]
    public void Error_Slug_TooLong()
    {
        static WorkspaceSlug act() => WorkspaceSlugBuilder.Build(isTooLong: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.SLUG_INVALID_LENGTH, exceptionMessage);
    }

    [Fact]
    public void Error_Slug_InvalidFormat()
    {
        static WorkspaceSlug act() => WorkspaceSlugBuilder.Build(isInvalidFormat: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.SLUG_INVALID_FORMAT, exceptionMessage);
    }

    [Fact]
    public void Error_Slug_Reserved()
    {
        static WorkspaceSlug act() => WorkspaceSlugBuilder.Build(isReserved: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.SLUG_RESERVED, exceptionMessage);
    }
}
