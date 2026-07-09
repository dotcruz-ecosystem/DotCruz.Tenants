using DotCruz.Tenants.Domain.Exceptions.Resources;
using FluentValidation;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Queries.GetTenantById;

public class GetTenantByIdQueryValidator : AbstractValidator<GetTenantByIdQuery>
{
    public GetTenantByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(ResourceMessagesException.ID_EMPTY);
    }
}
