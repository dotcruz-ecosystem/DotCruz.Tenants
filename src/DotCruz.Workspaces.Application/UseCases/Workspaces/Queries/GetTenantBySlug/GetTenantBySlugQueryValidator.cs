using DotCruz.Workspaces.Domain.Exceptions.Resources;
using FluentValidation;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetTenantBySlug;

public class GetTenantBySlugQueryValidator : AbstractValidator<GetTenantBySlugQuery>
{
    public GetTenantBySlugQueryValidator()
    {
        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage(ResourceMessagesException.SLUG_EMPTY);
    }
}
