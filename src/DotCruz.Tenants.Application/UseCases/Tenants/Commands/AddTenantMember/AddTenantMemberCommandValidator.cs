using DotCruz.Tenants.Domain.Exceptions.Resources;
using FluentValidation;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.AddTenantMember;

public class AddTenantMemberCommandValidator : AbstractValidator<AddTenantMemberCommand>
{
    public AddTenantMemberCommandValidator()
    {
        RuleFor(x => x.TenantId)
            .NotEmpty().WithMessage(ResourceMessagesException.ID_EMPTY);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY)
            .EmailAddress().WithMessage(ResourceMessagesException.EMAIL_INVALID);

        RuleFor(x => x.Role)
            .IsInEnum().WithMessage(ResourceMessagesException.ROLE_REQUIRED);
    }
}
