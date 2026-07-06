using CommonTestUtilities.ValueObjects.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using DotCruz.Tenants.Domain.ValueObjects.Tenants;

namespace Domain.Test.ValueObjects.Tenants;

public class TenantSlugTests
{
    [Fact]
    public void Success()
    {
        var slug = TenantSlugBuilder.Build();

        Assert.NotNull(slug);
        Assert.NotEmpty(slug.Value);
        Assert.Equal(slug.Value, slug.ToString());
    }

    [Fact]
    public void Success_Casing_Normalized()
    {
        var slug = new TenantSlug("My-Awesome-Slug-123");

        Assert.Equal("my-awesome-slug-123", slug.Value);
    }

    [Fact]
    public void Error_Slug_Empty()
    {
        static TenantSlug act() => TenantSlugBuilder.Build(isEmpty: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.SLUG_EMPTY, exceptionMessage);
    }

    [Fact]
    public void Error_Slug_TooShort()
    {
        static TenantSlug act() => TenantSlugBuilder.Build(isTooShort: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.SLUG_INVALID_LENGTH, exceptionMessage);
    }

    [Fact]
    public void Error_Slug_TooLong()
    {
        static TenantSlug act() => TenantSlugBuilder.Build(isTooLong: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.SLUG_INVALID_LENGTH, exceptionMessage);
    }

    [Fact]
    public void Error_Slug_InvalidFormat()
    {
        static TenantSlug act() => TenantSlugBuilder.Build(isInvalidFormat: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.SLUG_INVALID_FORMAT, exceptionMessage);
    }

    [Fact]
    public void Error_Slug_Reserved()
    {
        static TenantSlug act() => TenantSlugBuilder.Build(isReserved: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.SLUG_RESERVED, exceptionMessage);
    }
}
