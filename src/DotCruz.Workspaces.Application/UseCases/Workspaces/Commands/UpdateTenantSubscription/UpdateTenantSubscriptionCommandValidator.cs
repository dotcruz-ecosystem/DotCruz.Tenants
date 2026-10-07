using DotCruz.Workspaces.Domain.Exceptions.Resources;
using FluentValidation;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.UpdateWorkspaceSubscription;

public class UpdateWorkspaceSubscriptionCommandValidator : AbstractValidator<UpdateWorkspaceSubscriptionCommand>
{
    public UpdateWorkspaceSubscriptionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(ResourceMessagesException.ID_EMPTY);

        RuleFor(x => x.Plan)
            .IsInEnum().WithMessage(ResourceMessagesException.PLAN_INVALID);
    }
}
