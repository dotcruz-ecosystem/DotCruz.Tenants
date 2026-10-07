using DotCruz.Workspaces.Application.Abstractions.Data.Repositories.Workspaces;
using DotCruz.Workspaces.Application.DTOs.Workspaces;
using DotCruz.Workspaces.Application.Mappers.Workspaces;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;
using MediatR;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetTenantSummary;

public class GetTenantSummaryQueryHandler : IRequestHandler<GetTenantSummaryQuery, WorkspaceSummaryDto>
{
    private readonly IWorkspaceReadRepository _WorkspaceReadRepository;

    public GetTenantSummaryQueryHandler(IWorkspaceReadRepository WorkspaceReadRepository)
    {
        _WorkspaceReadRepository = WorkspaceReadRepository;
    }

    public async Task<WorkspaceSummaryDto> Handle(GetTenantSummaryQuery request, CancellationToken cancellationToken)
    {
        var Workspace = await _WorkspaceReadRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(ResourceMessagesException.TENANT_NOT_FOUND);

        return Workspace.ToSummaryDto();
    }
}
