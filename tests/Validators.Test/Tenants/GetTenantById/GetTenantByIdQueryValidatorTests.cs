using CommonTestUtilities.Queries.Tenants;
using DotCruz.Tenants.Application.UseCases.Tenants.GetTenantById;
using DotCruz.Tenants.Domain.Exceptions.Resources;

namespace Validators.Test.Tenants.GetTenantById;

public class GetTenantByIdQueryValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = GetTenantByIdQueryBuilder.Build();

        var result = new GetTenantByIdQueryValidator().Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Error_Id_Empty()
    {
        var request = GetTenantByIdQueryBuilder.Build(id: Guid.Empty);

        var result = new GetTenantByIdQueryValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.ID_EMPTY, error.ErrorMessage);
    }
}
