using DotCruz.Workspaces.Application.Extensions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;
using DotCruz.Workspaces.Domain.ValueObjects.Location;
using FluentValidation;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.UpdateWorkspaceAddress;

public class UpdateWorkspaceAddressCommandValidator : AbstractValidator<UpdateWorkspaceAddressCommand>
{
    public UpdateWorkspaceAddressCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(ResourceMessagesException.ID_EMPTY);

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
    }
}
