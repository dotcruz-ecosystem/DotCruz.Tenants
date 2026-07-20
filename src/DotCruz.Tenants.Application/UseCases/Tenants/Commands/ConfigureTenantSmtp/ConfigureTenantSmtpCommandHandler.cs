using DotCruz.Shared.Security.Context;
using DotCruz.Tenants.Application.Abstractions.Services.Smtp;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.ConfigureTenantSmtp;

public class ConfigureTenantSmtpCommandHandler : IRequestHandler<ConfigureTenantSmtpCommand>
{
    private readonly ISmtpConfigService _smtpConfigService;
    private readonly ISecurityContext _securityContext;

    public ConfigureTenantSmtpCommandHandler(
        ISmtpConfigService smtpConfigService,
        ISecurityContext securityContext
    )
    {
        _smtpConfigService = smtpConfigService;
        _securityContext = securityContext;
    }

    public async Task Handle(ConfigureTenantSmtpCommand request, CancellationToken cancellationToken)
    {
        if (_securityContext.TenantId != request.TenantId)
            throw new ForbiddenException(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE);

        await _smtpConfigService.SaveAsync(
            request.TenantId,
            request.Request.Host,
            request.Request.Port,
            request.Request.Username,
            request.Request.Password,
            request.Request.FromName,
            cancellationToken
        );
    }
}
