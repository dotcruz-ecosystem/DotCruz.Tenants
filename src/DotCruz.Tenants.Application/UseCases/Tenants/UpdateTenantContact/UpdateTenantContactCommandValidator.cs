using DotCruz.Tenants.Application.Extensions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using DotCruz.Tenants.Domain.ValueObjects.Communication;
using FluentValidation;

namespace DotCruz.Tenants.Application.UseCases.Tenants.UpdateTenantContact;

public class UpdateTenantContactCommandValidator : AbstractValidator<UpdateTenantContactCommand>
{
    public UpdateTenantContactCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(ResourceMessagesException.ID_EMPTY);

        RuleFor(x => x.TenantContact)
            .NotNull().WithMessage(ResourceMessagesException.CONTACT_REQUIRED);

        When(x => x.TenantContact != null, () =>
        {
            RuleFor(x => x.TenantContact.Email)
                .NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY)
                .MustBeValid(Email.Create);

            RuleFor(x => x.TenantContact.PhoneCountryCode)
                .InclusiveBetween(1, 999).WithMessage(ResourceMessagesException.PHONE_COUNTRY_CODE_INVALID);

            RuleFor(x => x.TenantContact.PhoneNationalNumber)
                .NotEmpty().WithMessage(ResourceMessagesException.PHONE_EMPTY)
                .MustBeValid((cmd, phone) =>
                {
                    if (cmd.TenantContact.PhoneCountryCode <= 0) return null!;
                    return PhoneNumber.Create(cmd.TenantContact.PhoneCountryCode, phone);
                });
        });
    }
}
