using DotCruz.Workspaces.Application.Abstractions.Data.Repositories.Workspaces;
using DotCruz.Workspaces.Application.DTOs.Workspaces;
using MediatR;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetUserWorkspaces;

public class GetUserWorkspacesQueryHandler : IRequestHandler<GetUserWorkspacesQuery, IReadOnlyCollection<UserWorkspaceDto>>
{
    private readonly IWorkspaceReadRepository _WorkspaceReadRepository;

    public GetUserWorkspacesQueryHandler(IWorkspaceReadRepository WorkspaceReadRepository)
    {
        _WorkspaceReadRepository = WorkspaceReadRepository;
    }

    public async Task<IReadOnlyCollection<UserWorkspaceDto>> Handle(GetUserWorkspacesQuery request, CancellationToken cancellationToken)
    {
        var workspaces = await _WorkspaceReadRepository.GetUserWorkspacesAsync(request.UserId, cancellationToken);

        return workspaces.Select(w => new UserWorkspaceDto(
            w.Workspace.Id,
            w.Workspace.Name.Value,
            w.Workspace.Slug.Value,
            w.Workspace.Type.ToString(),
            w.Workspace.Status.ToString(),
            w.Member.Role.ToString(),
            w.Member.Status.ToString(),
            w.Workspace.Subscription.Plan.ToString()
        )).ToList().AsReadOnly();
    }
}
