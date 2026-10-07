using DotCruz.Shared.Security.Authorization;
using DotCruz.Workspaces.Application.DTOs.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotCruz.Workspaces.Api.Controllers.Base;

[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
[Authorize(Policy = SecurityPolicies.AdminOnly)]
public class WorkspaceBaseController : ControllerBase
{
}
