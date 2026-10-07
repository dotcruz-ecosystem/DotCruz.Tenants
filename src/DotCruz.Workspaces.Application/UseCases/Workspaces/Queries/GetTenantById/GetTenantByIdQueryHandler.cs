using DotCruz.Workspaces.Application.Abstractions.Data.Repositories.Workspaces;
using DotCruz.Workspaces.Application.DTOs.Workspaces;
using DotCruz.Workspaces.Application.Mappers.Workspaces;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;
using MediatR;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetTenantById;

public class GetTenantByIdQueryHandler : IRequestHandler<GetTenantByIdQuery, WorkspaceDto>
{
    private readonly IWorkspaceReadRepository _WorkspaceReadRepository;

    public GetTenantByIdQueryHandler(IWorkspaceReadRepository WorkspaceReadRepository)
    {
        _WorkspaceReadRepository = WorkspaceReadRepository;
    }

    public async Task<WorkspaceDto> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
    {
        var Workspace = await _WorkspaceReadRepository.GetByIdAsync(request.Id, cancellationToken);

        if (Workspace is null)
            throw new NotFoundException(ResourceMessagesException.TENANT_NOT_FOUND);

        return Workspace.ToDto();
    }
}
