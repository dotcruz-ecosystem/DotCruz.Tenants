using CommonTestUtilities.Queries.Tenants;
using DotCruz.Tenants.Application.UseCases.Tenants.GetTenantBySlug;
using DotCruz.Tenants.Domain.Exceptions.Resources;

namespace Validators.Test.Tenants.GetTenantBySlug;

public class GetTenantBySlugQueryValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = GetTenantBySlugQueryBuilder.Build();

        var result = new GetTenantBySlugQueryValidator().Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Error_Slug_Empty()
    {
        var request = GetTenantBySlugQueryBuilder.Build(slug: string.Empty);

        var result = new GetTenantBySlugQueryValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.SLUG_EMPTY, error.ErrorMessage);
    }
}
