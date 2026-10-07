using DotCruz.Workspaces.Application.Extensions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;
using DotCruz.Workspaces.Domain.ValueObjects.Communication;
using FluentValidation;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.UpdateWorkspaceContact;

public class UpdateWorkspaceContactCommandValidator : AbstractValidator<UpdateWorkspaceContactCommand>
{
    public UpdateWorkspaceContactCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(ResourceMessagesException.ID_EMPTY);

        RuleFor(x => x.WorkspaceContact)
            .NotNull().WithMessage(ResourceMessagesException.CONTACT_REQUIRED);

        When(x => x.WorkspaceContact != null, () =>
        {
            RuleFor(x => x.WorkspaceContact.Email)
                .NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY)
                .MustBeValid(Email.Create);

            RuleFor(x => x.WorkspaceContact.PhoneCountryCode)
                .InclusiveBetween(1, 999).WithMessage(ResourceMessagesException.PHONE_COUNTRY_CODE_INVALID);

            RuleFor(x => x.WorkspaceContact.PhoneNationalNumber)
                .NotEmpty().WithMessage(ResourceMessagesException.PHONE_EMPTY)
                .MustBeValid((cmd, phone) =>
                {
                    if (cmd.WorkspaceContact.PhoneCountryCode <= 0) return null!;
                    return PhoneNumber.Create(cmd.WorkspaceContact.PhoneCountryCode, phone);
                });
        });
    }
}
