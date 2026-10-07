using DotCruz.Workspaces.Domain.Exceptions.Resources;
using FluentValidation;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetUserWorkspaces;

public class GetUserWorkspacesQueryValidator : AbstractValidator<GetUserWorkspacesQuery>
{
    public GetUserWorkspacesQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage(ResourceMessagesException.USER_ID_REQUIRED);
    }
}
