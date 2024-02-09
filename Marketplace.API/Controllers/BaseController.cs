using System.Security.Claims;

namespace Marketplace.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class BaseController : ControllerBase
{
	protected string? GetUserId()
	{
		return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
	}
}
