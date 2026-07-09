using DotCruz.Tenants.Domain.Exceptions.Resources;
using FluentValidation;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Queries.SearchTenants;

public class SearchTenantsQueryValidator : AbstractValidator<SearchTenantsQuery>
{
    public SearchTenantsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage(ResourceMessagesException.PAGE_NUMBER_INVALID);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage(ResourceMessagesException.PAGE_SIZE_INVALID);
    }
}
