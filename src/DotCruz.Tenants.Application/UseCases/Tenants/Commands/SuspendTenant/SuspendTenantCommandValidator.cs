using DotCruz.Tenants.Application.Extensions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using DotCruz.Tenants.Domain.ValueObjects.Tenants;
using FluentValidation;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.SuspendTenant;

public class SuspendTenantCommandValidator : AbstractValidator<SuspendTenantCommand>
{
    public SuspendTenantCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(ResourceMessagesException.ID_EMPTY);

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage(ResourceMessagesException.SUSPENSION_REASON_EMPTY)
            .MustBeValid(SuspensionReason.Create);
    }
}
