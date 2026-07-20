namespace DotCruz.Tenants.Application.DTOs.Tenants;

public sealed record ConfigureTenantSmtpDto(
    string Host,
    int Port,
    string Username,
    string Password,
    string FromName
);
