using DotCruz.Tenants.Application.DTOs.Base;
using DotCruz.Tenants.Application.DTOs.Tenants;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.ActivateTenant;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.CreateTenant;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.DeactivateTenant;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.SuspendTenant;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.TerminateTenant;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.UpdateTenantAddress;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.UpdateTenantContact;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.UpdateTenantSubscription;
using DotCruz.Tenants.Application.UseCases.Tenants.Queries.GetTenantById;
using DotCruz.Tenants.Application.UseCases.Tenants.Queries.GetTenantBySlug;
using DotCruz.Tenants.Application.UseCases.Tenants.Queries.SearchTenants;
using DotCruz.Tenants.Domain.Enums.Tenants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DotCruz.Tenants.Api.Controllers.Tenants;

[Route("api/[controller]")]
[ApiController]
public class TenantController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(SearchTenantsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] SearchTenantsQuery query, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [Route("{Id:guid}")]
    [ProducesResponseType(typeof(TenantDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid Id, CancellationToken cancellationToken)
    {
        var query = new GetTenantByIdQuery(Id);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [Route("{Slug}/slug")]
    [ProducesResponseType(typeof(TenantDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBySlug([FromRoute] string Slug, CancellationToken cancellationToken)
    {
        var query = new GetTenantBySlugQuery(Slug);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> Post([FromBody] CreateTenantCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { Id = result }, result);
    }

    [HttpPatch]
    [Route("{Id:guid}/activate")]
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAddress([FromRoute] Guid Id, [FromBody] TenantAddressDto request, CancellationToken cancellationToken)
    {
        var command = new UpdateTenantAddressCommand(Id, request);
        await mediator.Send(command, cancellationToken); 
        return NoContent();
    }

    [HttpPatch]
    [Route("{Id:guid}/contact")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateContact([FromRoute] Guid Id, [FromBody] TenantContactDto request, CancellationToken cancellationToken)
    {
        var command = new UpdateTenantContactCommand(Id, request);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPatch]
    [Route("{Id:guid}/subscription")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateSubscription([FromRoute] Guid Id, [FromBody] PlanType request, CancellationToken cancellationToken)
    {
        var command = new UpdateTenantSubscriptionCommand(Id, request);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete]
    [Route("{Id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete([FromRoute] Guid Id, CancellationToken cancellationToken)
    {
        var command = new TerminateTenantCommand(Id);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }
}
