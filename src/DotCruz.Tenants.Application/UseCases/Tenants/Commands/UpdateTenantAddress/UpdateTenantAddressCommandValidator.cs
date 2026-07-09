using DotCruz.Tenants.Application.Extensions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using DotCruz.Tenants.Domain.ValueObjects.Location;
using FluentValidation;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.UpdateTenantAddress;

public class UpdateTenantAddressCommandValidator : AbstractValidator<UpdateTenantAddressCommand>
{
    public UpdateTenantAddressCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(ResourceMessagesException.ID_EMPTY);

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
    }
}
