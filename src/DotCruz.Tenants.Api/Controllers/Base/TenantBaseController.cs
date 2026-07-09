using DotCruz.Tenants.Application.DTOs.Base;
using Microsoft.AspNetCore.Mvc;

namespace DotCruz.Tenants.Api.Controllers.Base;

[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
public class TenantBaseController : ControllerBase
{
}
