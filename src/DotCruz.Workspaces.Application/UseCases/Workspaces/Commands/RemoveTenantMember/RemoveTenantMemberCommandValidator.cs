using DotCruz.Workspaces.Domain.Exceptions.Resources;
using FluentValidation;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.RemoveWorkspaceMember;

public class RemoveWorkspaceMemberCommandValidator : AbstractValidator<RemoveWorkspaceMemberCommand>
{
    public RemoveWorkspaceMemberCommandValidator()
    {
        RuleFor(x => x.WorkspaceId)
            .NotEmpty().WithMessage(ResourceMessagesException.ID_EMPTY);

        RuleFor(x => x.MemberId)
            .NotEmpty().WithMessage(ResourceMessagesException.ID_EMPTY);
    }
}
