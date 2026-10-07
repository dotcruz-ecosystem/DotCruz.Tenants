using DotCruz.Workspaces.Application.Extensions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;
using DotCruz.Workspaces.Domain.ValueObjects.Communication;
using DotCruz.Workspaces.Domain.ValueObjects.Fiscal;
using DotCruz.Workspaces.Domain.ValueObjects.Identity;
using DotCruz.Workspaces.Domain.ValueObjects.Location;
using DotCruz.Workspaces.Domain.ValueObjects.Workspaces;
using FluentValidation;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.CreateTenant;

public class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
{
    public CreateTenantCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY)
            .MustBeValid(Name.Create);

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage(ResourceMessagesException.SLUG_EMPTY)
            .MustBeValid(WorkspaceSlug.Create);

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

        RuleFor(x => x.WorkspaceAddress)
            .NotNull().WithMessage(ResourceMessagesException.ADDRESS_REQUIRED);

        When(x => x.WorkspaceAddress != null, () =>
        {
            RuleFor(x => x.WorkspaceAddress.Street)
                .NotEmpty().WithMessage(ResourceMessagesException.STREET_EMPTY);

            RuleFor(x => x.WorkspaceAddress.Number)
                .NotEmpty().WithMessage(ResourceMessagesException.NUMBER_EMPTY);

            RuleFor(x => x.WorkspaceAddress.Neighborhood)
                .NotEmpty().WithMessage(ResourceMessagesException.NEIGHBORHOOD_EMPTY);

            RuleFor(x => x.WorkspaceAddress.City)
                .NotEmpty().WithMessage(ResourceMessagesException.CITY_EMPTY);

            RuleFor(x => x.WorkspaceAddress.State)
                .NotEmpty().WithMessage(ResourceMessagesException.STATE_REQUIRED)
                .MustBeValid(State.Create);

            RuleFor(x => x.WorkspaceAddress.ZipCode)
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
