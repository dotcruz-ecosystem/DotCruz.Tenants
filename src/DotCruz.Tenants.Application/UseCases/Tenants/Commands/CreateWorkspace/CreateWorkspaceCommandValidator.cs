using DotCruz.Tenants.Application.Extensions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using DotCruz.Tenants.Domain.ValueObjects.Communication;
using DotCruz.Tenants.Domain.ValueObjects.Identity;
using DotCruz.Tenants.Domain.ValueObjects.Tenants;
using FluentValidation;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.CreateWorkspace;

public class CreateWorkspaceCommandValidator : AbstractValidator<CreateWorkspaceCommand>
{
    public CreateWorkspaceCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY)
            .MustBeValid(Name.Create);

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage(ResourceMessagesException.SLUG_EMPTY)
            .MustBeValid(TenantSlug.Create);

        RuleFor(x => x.Type)
            .IsInEnum();

        When(x => !string.IsNullOrWhiteSpace(x.OwnerEmail), () =>
        {
            RuleFor(x => x.OwnerEmail!)
                .MustBeValid(Email.Create);
        });
    }
}
