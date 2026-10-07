using DotCruz.Workspaces.Domain.Exceptions.Resources;
using FluentValidation;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.DeactivateTenant;

public class DeactivateTenantCommandValidator : AbstractValidator<DeactivateTenantCommand>
{
    public DeactivateTenantCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(ResourceMessagesException.ID_EMPTY);
    }
}
