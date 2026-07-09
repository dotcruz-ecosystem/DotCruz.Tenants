using DotCruz.Tenants.Application.Abstractions.Data;
using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;
using DotCruz.Tenants.Application.Events.CreatedTenant;
using DotCruz.Tenants.Application.Mappers.Fiscal;
using DotCruz.Tenants.Application.Mappers.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.CreateTenant;

public class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, Guid>
{
    private readonly ITenantWriteRepository _tenantWriteRepository;
    private readonly ITenantReadRepository _tenantReadRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public CreateTenantCommandHandler(
        ITenantWriteRepository tenantWriteRepository,
        ITenantReadRepository tenantReadRepository,
        IUnitOfWork unitOfWork,
        IMediator mediator)
    {
        _tenantWriteRepository = tenantWriteRepository;
        _tenantReadRepository = tenantReadRepository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        await AfterValidation(request, cancellationToken);

        var tenant = request.ToDomain();

        await _tenantWriteRepository.AddAsync(tenant);

        await _mediator.Publish(new CreatedTenantEvent(tenant.Id, request), cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return tenant.Id;
    }

    private async Task AfterValidation(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        var errors = new List<string>();

        if (await _tenantReadRepository.ExistsWithSlugAsync(request.Slug, cancellationToken))
            errors.Add(ResourceMessagesException.SLUG_ALREADY_EXISTS);

        if (await _tenantReadRepository.ExistsWithDocumentAsync(request.TenantDocument.ToDomain().Number, cancellationToken))
            errors.Add(ResourceMessagesException.DOCUMENT_ALREADY_EXISTS);

        if (errors.Count != 0)
            throw new ErrorOnValidationException(errors);
    }
}
