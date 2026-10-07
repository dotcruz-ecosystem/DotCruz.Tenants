using DotCruz.Workspaces.Application.Extensions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;
using DotCruz.Workspaces.Domain.ValueObjects.Workspaces;
using FluentValidation;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.SuspendTenant;

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
