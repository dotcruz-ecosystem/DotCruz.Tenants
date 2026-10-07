using DotCruz.Workspaces.Application.Abstractions.Data;
using DotCruz.Workspaces.Application.Abstractions.Data.Repositories.Workspaces;
using DotCruz.Workspaces.Application.Events.CreatedTenant;
using DotCruz.Workspaces.Application.Mappers.Fiscal;
using DotCruz.Workspaces.Application.Mappers.Workspaces;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;
using MediatR;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.CreateTenant;

public class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, Guid>
{
    private readonly IWorkspaceWriteRepository _WorkspaceWriteRepository;
    private readonly IWorkspaceReadRepository _WorkspaceReadRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public CreateTenantCommandHandler(
        IWorkspaceWriteRepository WorkspaceWriteRepository,
        IWorkspaceReadRepository WorkspaceReadRepository,
        IUnitOfWork unitOfWork,
        IMediator mediator)
    {
        _WorkspaceWriteRepository = WorkspaceWriteRepository;
        _WorkspaceReadRepository = WorkspaceReadRepository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        await AfterValidation(request, cancellationToken);

        var Workspace = request.ToDomain();

        await _WorkspaceWriteRepository.AddAsync(Workspace);

        await _mediator.Publish(new CreatedTenantEvent(Workspace.Id, request), cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return Workspace.Id;
    }

    private async Task AfterValidation(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        var errors = new List<string>();

        if (await _WorkspaceReadRepository.ExistsWithSlugAsync(request.Slug, cancellationToken))
            errors.Add(ResourceMessagesException.SLUG_ALREADY_EXISTS);

        if (await _WorkspaceReadRepository.ExistsWithDocumentAsync(request.TenantDocument.ToDomain().Number, cancellationToken))
            errors.Add(ResourceMessagesException.DOCUMENT_ALREADY_EXISTS);

        if (errors.Count != 0)
            throw new ErrorOnValidationException(errors);
    }
}
