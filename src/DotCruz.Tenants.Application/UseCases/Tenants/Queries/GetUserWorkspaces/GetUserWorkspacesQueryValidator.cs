using DotCruz.Tenants.Domain.Exceptions.Resources;
using FluentValidation;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Queries.GetUserWorkspaces;

public class GetUserWorkspacesQueryValidator : AbstractValidator<GetUserWorkspacesQuery>
{
    public GetUserWorkspacesQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage(ResourceMessagesException.USER_ID_REQUIRED);
    }
}
