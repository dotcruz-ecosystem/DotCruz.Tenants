using DotCruz.Tenants.Domain.Exceptions.Resources;
using FluentValidation;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.RemoveTenantMember;

public class RemoveTenantMemberCommandValidator : AbstractValidator<RemoveTenantMemberCommand>
{
    public RemoveTenantMemberCommandValidator()
    {
        RuleFor(x => x.TenantId)
            .NotEmpty().WithMessage(ResourceMessagesException.ID_EMPTY);

        RuleFor(x => x.MemberId)
            .NotEmpty().WithMessage(ResourceMessagesException.ID_EMPTY);
    }
}
