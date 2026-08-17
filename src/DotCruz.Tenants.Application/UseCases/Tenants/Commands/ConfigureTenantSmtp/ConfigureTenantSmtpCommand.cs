using DotCruz.Tenants.Application.DTOs.Tenants;
using MediatR;
using DotCruz.Tenants.Application.Abstractions.Security;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.ConfigureTenantSmtp;

public sealed record ConfigureTenantSmtpCommand(
    Guid TenantId,
    ConfigureTenantSmtpDto Request
) : IRequest, ITenantScopedRequest;
