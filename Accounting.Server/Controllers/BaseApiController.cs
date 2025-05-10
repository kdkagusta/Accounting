using Accounting.Server.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Accounting.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        protected IActionResult ResponseOk<T>(T data, string message = "Success")
        {
            return Ok(ApiResponse<T>.Ok(data, message));
        }

        protected IActionResult ResponseError(string message, List<string> errors = null)
        {
            return BadRequest(ApiResponse<object>.Fail(message, errors));
        }
    }
}
