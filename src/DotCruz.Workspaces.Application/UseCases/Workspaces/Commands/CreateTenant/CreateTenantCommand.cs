using DotCruz.Workspaces.Application.DTOs.Fiscal;
using DotCruz.Workspaces.Application.DTOs.Workspaces;
using MediatR;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.CreateTenant;

public sealed record CreateTenantCommand(
    string Name,
    string Slug,
    FiscalDocumentDto TenantDocument,
    WorkspaceContactDto WorkspaceContact,
    WorkspaceAddressDto WorkspaceAddress,
    WorkspaceAdminUserDto TenantAdminUser
) : IRequest<Guid>;
