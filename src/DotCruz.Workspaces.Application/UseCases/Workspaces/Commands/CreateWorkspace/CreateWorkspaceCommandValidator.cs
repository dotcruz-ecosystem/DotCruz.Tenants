using DotCruz.Workspaces.Application.Extensions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;
using DotCruz.Workspaces.Domain.ValueObjects.Communication;
using DotCruz.Workspaces.Domain.ValueObjects.Identity;
using DotCruz.Workspaces.Domain.ValueObjects.Workspaces;
using FluentValidation;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.CreateWorkspace;

public class CreateWorkspaceCommandValidator : AbstractValidator<CreateWorkspaceCommand>
{
    public CreateWorkspaceCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY)
            .MustBeValid(Name.Create);

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage(ResourceMessagesException.SLUG_EMPTY)
            .MustBeValid(WorkspaceSlug.Create);

        RuleFor(x => x.Type)
            .IsInEnum();

        When(x => !string.IsNullOrWhiteSpace(x.OwnerEmail), () =>
        {
            RuleFor(x => x.OwnerEmail!)
                .MustBeValid(Email.Create);
        });
    }
}
