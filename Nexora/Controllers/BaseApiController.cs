using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Nexora.Core.Common.Models;

namespace Nexora.Api.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(typeof(ErrorResultModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResultModel), StatusCodes.Status500InternalServerError)]
    public abstract class BaseApiController : ControllerBase
    {
        protected IActionResult OkResult<T>(T data) => Ok(data);

        protected IActionResult CreatedResult<T>(T data) => StatusCode(201, data);

        protected IActionResult NoContentResult() => NoContent();
    }
}
