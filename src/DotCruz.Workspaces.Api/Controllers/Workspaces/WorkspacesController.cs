using DotCruz.Shared.Security.Authorization;
using DotCruz.Workspaces.Api.Controllers.Base;
using DotCruz.Workspaces.Application.DTOs.Base;
using DotCruz.Workspaces.Application.DTOs.Workspaces;
using DotCruz.Workspaces.Application.DTOs.Storage;
using DotCruz.Workspaces.Application.Abstractions.Services.Storage;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.ActivateTenant;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.CreateTenant;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.DeactivateTenant;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.SuspendTenant;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.TerminateTenant;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.UpdateWorkspaceAddress;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.UpdateWorkspaceContact;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.UpdateWorkspaceSubscription;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.UpdateWorkspaceBranding;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.ConfigureTenantSmtp;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetTenantById;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetTenantSummary;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetTenantBySlug;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.SearchTenants;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetUploadUrl;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.AddWorkspaceMember;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.RemoveWorkspaceMember;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetWorkspaceMembers;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetUserWorkspaces;
using DotCruz.Workspaces.Domain.Enums.Workspaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DotCruz.Workspaces.Application.Abstractions.Services.Storage.Responses;

namespace DotCruz.Workspaces.Api.Controllers.Workspaces;

public class WorkspacesController(IMediator mediator) : WorkspaceBaseController
{
    [HttpGet]
    [Authorize(Policy = SecurityPolicies.AdminOnly)]
    [ProducesResponseType(typeof(SearchTenantsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] SearchTenantsQuery query, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [Route("{Id:guid}")]
    [Authorize(Policy = SecurityPolicies.UserOrService)]
    [ProducesResponseType(typeof(WorkspaceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid Id, CancellationToken cancellationToken)
    {
        var query = new GetTenantByIdQuery(Id);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [Route("{Id:guid}/summary")]
    [Authorize(Policy = SecurityPolicies.UserOrService)]
    [ProducesResponseType(typeof(WorkspaceSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSummary([FromRoute] Guid Id, CancellationToken cancellationToken)
    {
        var query = new GetTenantSummaryQuery(Id);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [Route("{Slug}/slug")]
    [Authorize(Policy = SecurityPolicies.AdminOnly)]
    [ProducesResponseType(typeof(WorkspaceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBySlug([FromRoute] string Slug, CancellationToken cancellationToken)
    {
        var query = new GetTenantBySlugQuery(Slug);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = SecurityPolicies.ServiceOnly)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> Post([FromBody] CreateTenantCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { Id = result }, result);
    }

    [HttpPatch]
    [Route("{Id:guid}/activate")]
    [Authorize(Policy = SecurityPolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Activate([FromRoute] Guid Id, CancellationToken cancellationToken)
    {
        var command = new ActivateTenantCommand(Id);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPatch]
    [Route("{Id:guid}/deactivate")]
    [Authorize(Policy = SecurityPolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Deactivate([FromRoute] Guid Id, CancellationToken cancellationToken)
    {
        var command = new DeactivateTenantCommand(Id);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPatch]
    [Route("{Id:guid}/suspend")]
    [Authorize(Policy = SecurityPolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Suspend([FromRoute] Guid Id, [FromBody] string reason, CancellationToken cancellationToken)
    {
        var command = new SuspendTenantCommand(Id, reason);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPatch]
    [Route("{Id:guid}/address")]
    [Authorize(Policy = SecurityPolicies.WorkspaceAdminOrAdmin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAddress([FromRoute] Guid Id, [FromBody] WorkspaceAddressDto request, CancellationToken cancellationToken)
    {
        var command = new UpdateWorkspaceAddressCommand(Id, request);
        await mediator.Send(command, cancellationToken); 
        return NoContent();
    }

    [HttpPatch]
    [Route("{Id:guid}/contact")]
    [Authorize(Policy = SecurityPolicies.WorkspaceAdminOrAdmin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateContact([FromRoute] Guid Id, [FromBody] WorkspaceContactDto request, CancellationToken cancellationToken)
    {
        var command = new UpdateWorkspaceContactCommand(Id, request);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPatch]
    [Route("{Id:guid}/subscription")]
    [Authorize(Policy = SecurityPolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateSubscription([FromRoute] Guid Id, [FromBody] PlanType request, CancellationToken cancellationToken)
    {
        var command = new UpdateWorkspaceSubscriptionCommand(Id, request);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPost]
    [Route("{Id:guid}/upload-url")]
    [Authorize(Policy = SecurityPolicies.WorkspaceAdminOrAdmin)]
    [ProducesResponseType(typeof(StorageUploadUrlDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUploadUrl([FromRoute] Guid Id, [FromBody] GetUploadUrlRequest request, CancellationToken cancellationToken)
    {
        var query = new GetUploadUrlQuery(Id, request.Purpose, request.FileName, request.ContentType);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPatch]
    [Route("{Id:guid}/branding")]
    [Authorize(Policy = SecurityPolicies.WorkspaceAdminOrAdmin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateBranding([FromRoute] Guid Id, [FromBody] WorkspaceBrandingDto request, CancellationToken cancellationToken)
    {
        var command = new UpdateWorkspaceBrandingCommand(Id, request);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPatch]
    [Route("{Id:guid}/smtp")]
    [Authorize(Policy = SecurityPolicies.WorkspaceAdminOrAdmin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConfigureSmtp([FromRoute] Guid Id, [FromBody] ConfigureWorkspaceSmtpDto request, CancellationToken cancellationToken)
    {
        var command = new ConfigureTenantSmtpCommand(Id, request);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete]
    [Route("{Id:guid}")]
    [Authorize(Policy = SecurityPolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete([FromRoute] Guid Id, CancellationToken cancellationToken)
    {
        var command = new TerminateTenantCommand(Id);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPost]
    [Route("{Id:guid}/members")]
    [Authorize(Policy = SecurityPolicies.WorkspaceAdminOrAdmin)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddMember([FromRoute] Guid Id, [FromBody] AddWorkspaceMemberRequest request, CancellationToken cancellationToken)
    {
        var role = Enum.TryParse<WorkspaceRole>(request.Role, true, out var parsedRole) ? parsedRole : WorkspaceRole.Member;
        var command = new AddWorkspaceMemberCommand(Id, request.Email, role, request.UserId);
        var result = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetMembers), new { Id }, result);
    }

    [HttpGet]
    [Route("{Id:guid}/members")]
    [Authorize(Policy = SecurityPolicies.UserOrService)]
    [ProducesResponseType(typeof(IReadOnlyCollection<WorkspaceMemberDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMembers([FromRoute] Guid Id, CancellationToken cancellationToken)
    {
        var query = new GetWorkspaceMembersQuery(Id);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpDelete]
    [Route("{Id:guid}/members/{memberId:guid}")]
    [Authorize(Policy = SecurityPolicies.WorkspaceAdminOrAdmin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveMember([FromRoute] Guid Id, [FromRoute] Guid memberId, CancellationToken cancellationToken)
    {
        var command = new RemoveWorkspaceMemberCommand(Id, memberId);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpGet]
    [Route("users/{userId:guid}/workspaces")]
    [Authorize(Policy = SecurityPolicies.UserOrService)]
    [ProducesResponseType(typeof(IReadOnlyCollection<UserWorkspaceDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserWorkspaces([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        var query = new GetUserWorkspacesQuery(userId);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Route("workspaces")]
    [Authorize(Policy = SecurityPolicies.UserOrService)]
    [ProducesResponseType(typeof(UserWorkspaceDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateWorkspace([FromBody] DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.CreateWorkspace.CreateWorkspaceCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { Id = result.WorkspaceId }, result);
    }
}
