using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace HamroCommunity.Configs
{
    [Route("api/[controller]")]
    [ApiController]
    public class HamroCommunityBaseController : ControllerBase
    {

        protected IActionResult HandleFailureResult(IEnumerable<string> errors)
        {
            // Check the error messages and return appropriate status code

            if (errors.Any(errors => errors.Contains("Unauthorized")))
            {
                return Unauthorized(errors);
            }
            else if (errors.Any(errors => errors.Contains("NotFound")))
            {
                return NotFound(errors);
            }
            else if (errors.Any(errors =>errors.Contains("InsufficientFunds")))
            {
                return StatusCode(402, errors);
            }
            else if (errors.Any(errors =>errors.Contains("ForbiddenAccess")))
            {
                return Forbid(string.Join(", ", errors));
            }
            else if(errors.Any(errors => errors.Contains("Conflict")))
            {
                return Conflict(errors);
            }
            else
            {
                return BadRequest(errors);
            }
        }
    }
}
