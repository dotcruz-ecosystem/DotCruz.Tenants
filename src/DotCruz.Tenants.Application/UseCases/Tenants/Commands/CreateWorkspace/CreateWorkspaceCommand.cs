using DotCruz.Tenants.Application.DTOs.Tenants;
using DotCruz.Tenants.Domain.Enums.Tenants;
using MediatR;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.CreateWorkspace;

public sealed record CreateWorkspaceCommand(
    string Name,
    string Slug,
    TenantType Type = TenantType.Personal,
    string? OwnerEmail = null
) : IRequest<UserWorkspaceDto>;
