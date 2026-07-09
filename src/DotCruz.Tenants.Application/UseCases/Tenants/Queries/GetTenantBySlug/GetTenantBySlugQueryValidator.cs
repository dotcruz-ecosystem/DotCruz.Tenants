using DotCruz.Tenants.Domain.Exceptions.Resources;
using FluentValidation;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Queries.GetTenantBySlug;

public class GetTenantBySlugQueryValidator : AbstractValidator<GetTenantBySlugQuery>
{
    public GetTenantBySlugQueryValidator()
    {
        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage(ResourceMessagesException.SLUG_EMPTY);
    }
}
