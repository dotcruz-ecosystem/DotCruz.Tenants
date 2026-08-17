using DotCruz.Tenants.Application.Abstractions.Services.Smtp;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.ConfigureTenantSmtp;

public class ConfigureTenantSmtpCommandHandler : IRequestHandler<ConfigureTenantSmtpCommand>
{
    private readonly ISmtpConfigService _smtpConfigService;

    public ConfigureTenantSmtpCommandHandler(ISmtpConfigService smtpConfigService)
    {
        _smtpConfigService = smtpConfigService;
    }

    public async Task Handle(ConfigureTenantSmtpCommand request, CancellationToken cancellationToken)
    {
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
