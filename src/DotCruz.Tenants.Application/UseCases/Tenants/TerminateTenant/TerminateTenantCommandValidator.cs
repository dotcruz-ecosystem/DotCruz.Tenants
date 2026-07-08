using DotCruz.Tenants.Domain.Exceptions.Resources;
using FluentValidation;

namespace DotCruz.Tenants.Application.UseCases.Tenants.TerminateTenant;

public class TerminateTenantCommandValidator : AbstractValidator<TerminateTenantCommand>
{
    public TerminateTenantCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(ResourceMessagesException.ID_EMPTY);
    }
}
