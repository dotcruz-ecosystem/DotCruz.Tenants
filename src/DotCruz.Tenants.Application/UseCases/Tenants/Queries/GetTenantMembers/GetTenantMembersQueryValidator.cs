using DotCruz.Tenants.Domain.Exceptions.Resources;
using FluentValidation;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Queries.GetTenantMembers;

public class GetTenantMembersQueryValidator : AbstractValidator<GetTenantMembersQuery>
{
    public GetTenantMembersQueryValidator()
    {
        RuleFor(x => x.TenantId)
            .NotEmpty().WithMessage(ResourceMessagesException.ID_EMPTY);
    }
}
