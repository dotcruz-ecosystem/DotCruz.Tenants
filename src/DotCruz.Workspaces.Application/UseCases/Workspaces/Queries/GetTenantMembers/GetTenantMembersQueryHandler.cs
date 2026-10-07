using DotCruz.Workspaces.Application.Abstractions.Data.Repositories.Workspaces;
using DotCruz.Workspaces.Application.DTOs.Workspaces;
using DotCruz.Workspaces.Application.Mappers.Workspaces;
using MediatR;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetWorkspaceMembers;

public class GetWorkspaceMembersQueryHandler : IRequestHandler<GetWorkspaceMembersQuery, IReadOnlyCollection<WorkspaceMemberDto>>
{
    private readonly IWorkspaceReadRepository _WorkspaceReadRepository;

    public GetWorkspaceMembersQueryHandler(IWorkspaceReadRepository WorkspaceReadRepository)
    {
        _WorkspaceReadRepository = WorkspaceReadRepository;
    }

    public async Task<IReadOnlyCollection<WorkspaceMemberDto>> Handle(GetWorkspaceMembersQuery request, CancellationToken cancellationToken)
    {
        var members = await _WorkspaceReadRepository.GetMembersAsync(request.WorkspaceId, cancellationToken);
        return members.Select(m => m.ToDto()).ToList().AsReadOnly();
    }
}
