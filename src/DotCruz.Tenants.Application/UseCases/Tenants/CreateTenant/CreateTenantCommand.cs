using DotCruz.Tenants.Application.DTOs.Fiscal;
using DotCruz.Tenants.Application.DTOs.Tenants;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.CreateTenant;

public sealed record CreateTenantCommand(
    string Name,
    string Slug,
    FiscalDocumentDto TenantDocument,
    TenantContactDto TenantContact,
    TenantAddressDto TenantAddress,
    TenantAdminUserDto TenantAdminUser
) : IRequest<Guid>;
