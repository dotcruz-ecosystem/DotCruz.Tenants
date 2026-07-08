using DotCruz.Tenants.Domain.Exceptions.Resources;
using FluentValidation;

namespace DotCruz.Tenants.Application.UseCases.Tenants.GetTenantById;

public class GetTenantByIdQueryValidator : AbstractValidator<GetTenantByIdQuery>
{
    public GetTenantByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(ResourceMessagesException.ID_EMPTY);
    }
}
