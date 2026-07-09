using DotCruz.Tenants.Domain.Exceptions.Resources;
using FluentValidation;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.DeactivateTenant;

public class DeactivateTenantCommandValidator : AbstractValidator<DeactivateTenantCommand>
{
    public DeactivateTenantCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(ResourceMessagesException.ID_EMPTY);
    }
}
