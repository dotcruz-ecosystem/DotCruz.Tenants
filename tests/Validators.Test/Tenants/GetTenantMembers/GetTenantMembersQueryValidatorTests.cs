using CommonTestUtilities.Queries.Tenants;
using DotCruz.Tenants.Application.UseCases.Tenants.Queries.GetTenantMembers;
using DotCruz.Tenants.Domain.Exceptions.Resources;

namespace Validators.Test.Tenants.GetTenantMembers;

public class GetTenantMembersQueryValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = GetTenantMembersQueryBuilder.Build();

        var result = new GetTenantMembersQueryValidator().Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Error_TenantId_Empty()
    {
        var request = GetTenantMembersQueryBuilder.Build(isTenantIdEmpty: true);

        var result = new GetTenantMembersQueryValidator().Validate(request);

        Assert.False(result.IsValid);
        var error = Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.ID_EMPTY, error.ErrorMessage);
    }
}
