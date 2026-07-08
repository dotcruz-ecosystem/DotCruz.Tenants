using CommonTestUtilities.Queries.Tenants;
using DotCruz.Tenants.Application.UseCases.Tenants.SearchTenants;
using DotCruz.Tenants.Domain.Exceptions.Resources;

namespace Validators.Test.Tenants.SearchTenants;

public class SearchTenantsQueryValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = SearchTenantsQueryBuilder.Build();

        var result = new SearchTenantsQueryValidator().Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Error_PageNumber_Invalid()
    {
        var request = SearchTenantsQueryBuilder.Build(pageNumber: 0);

        var result = new SearchTenantsQueryValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.PAGE_NUMBER_INVALID, error.ErrorMessage);
    }

    [Fact]
    public void Error_PageSize_Invalid_Too_Small()
    {
        var request = SearchTenantsQueryBuilder.Build(pageSize: 0);

        var result = new SearchTenantsQueryValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.PAGE_SIZE_INVALID, error.ErrorMessage);
    }

    [Fact]
    public void Error_PageSize_Invalid_Too_Large()
    {
        var request = SearchTenantsQueryBuilder.Build(pageSize: 101);

        var result = new SearchTenantsQueryValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.PAGE_SIZE_INVALID, error.ErrorMessage);
    }
}
