using DotCruz.Workspaces.Domain.Exceptions.Resources;
using FluentValidation;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetWorkspaceMembers;

public class GetWorkspaceMembersQueryValidator : AbstractValidator<GetWorkspaceMembersQuery>
{
    public GetWorkspaceMembersQueryValidator()
    {
        RuleFor(x => x.WorkspaceId)
            .NotEmpty().WithMessage(ResourceMessagesException.ID_EMPTY);
    }
}
