using DotCruz.Tenants.Application.Extensions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using DotCruz.Tenants.Domain.ValueObjects.Communication;
using DotCruz.Tenants.Domain.ValueObjects.Fiscal;
using DotCruz.Tenants.Domain.ValueObjects.Identity;
using DotCruz.Tenants.Domain.ValueObjects.Location;
using DotCruz.Tenants.Domain.ValueObjects.Tenants;
using FluentValidation;

namespace DotCruz.Tenants.Application.UseCases.Tenants.CreateTenant;

public class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
{
    public CreateTenantCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY)
            .MustBeValid(Name.Create);

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage(ResourceMessagesException.SLUG_EMPTY)
            .MustBeValid(TenantSlug.Create);

        RuleFor(x => x.TenantDocument)
            .NotNull().WithMessage(ResourceMessagesException.DOCUMENT_REQUIRED);

        When(x => x.TenantDocument != null, () =>
        {
            RuleFor(x => x.TenantDocument.DocumentNumber)
                .NotEmpty().WithMessage(ResourceMessagesException.DOCUMENT_EMPTY)
                .MustBeValid((cmd, doc) => FiscalDocument.Create(doc, cmd.TenantDocument.DocumentType));

            RuleFor(x => x.TenantDocument.DocumentType)
                .IsInEnum();
        });

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

        RuleFor(x => x.TenantAddress)
            .NotNull().WithMessage(ResourceMessagesException.ADDRESS_REQUIRED);

        When(x => x.TenantAddress != null, () =>
        {
            RuleFor(x => x.TenantAddress.Street)
                .NotEmpty().WithMessage(ResourceMessagesException.STREET_EMPTY);

            RuleFor(x => x.TenantAddress.Number)
                .NotEmpty().WithMessage(ResourceMessagesException.NUMBER_EMPTY);

            RuleFor(x => x.TenantAddress.Neighborhood)
                .NotEmpty().WithMessage(ResourceMessagesException.NEIGHBORHOOD_EMPTY);

            RuleFor(x => x.TenantAddress.City)
                .NotEmpty().WithMessage(ResourceMessagesException.CITY_EMPTY);

            RuleFor(x => x.TenantAddress.State)
                .NotEmpty().WithMessage(ResourceMessagesException.STATE_REQUIRED)
                .MustBeValid(State.Create);

            RuleFor(x => x.TenantAddress.ZipCode)
                .NotEmpty().WithMessage(ResourceMessagesException.ZIPCODE_REQUIRED)
                .MustBeValid(ZipCode.Create);
        });

        RuleFor(x => x.TenantAdminUser)
            .NotNull().WithMessage(ResourceMessagesException.ADMIN_USER_REQUIRED);

        When(x => x.TenantAdminUser != null, () =>
        {
            RuleFor(x => x.TenantAdminUser.AdminName)
                .NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY)
                .MustBeValid(Name.Create);

            RuleFor(x => x.TenantAdminUser.AdminEmail)
                .NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY)
                .MustBeValid(Email.Create);
        });
    }
}
