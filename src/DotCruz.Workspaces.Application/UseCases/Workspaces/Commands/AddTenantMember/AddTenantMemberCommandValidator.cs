using DotCruz.Workspaces.Domain.Exceptions.Resources;
using FluentValidation;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.AddWorkspaceMember;

public class AddWorkspaceMemberCommandValidator : AbstractValidator<AddWorkspaceMemberCommand>
{
    public AddWorkspaceMemberCommandValidator()
    {
        RuleFor(x => x.WorkspaceId)
            .NotEmpty().WithMessage(ResourceMessagesException.ID_EMPTY);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY)
            .EmailAddress().WithMessage(ResourceMessagesException.EMAIL_INVALID);

        RuleFor(x => x.Role)
            .IsInEnum().WithMessage(ResourceMessagesException.ROLE_REQUIRED);
    }
}
