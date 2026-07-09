using DotCruz.Tenants.Domain.Exceptions.Resources;
using FluentValidation;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.UpdateTenantSubscription;

public class UpdateTenantSubscriptionCommandValidator : AbstractValidator<UpdateTenantSubscriptionCommand>
{
    public UpdateTenantSubscriptionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(ResourceMessagesException.ID_EMPTY);

        RuleFor(x => x.Plan)
            .IsInEnum().WithMessage(ResourceMessagesException.PLAN_INVALID);
    }
}
