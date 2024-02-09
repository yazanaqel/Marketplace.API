using Marketplace.BAL.Services;

namespace Marketplace.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class VariantController(IVariantService variantService) : BaseController
{
    private readonly IVariantService _variantService = variantService;

    [HttpDelete("DeleteVariant")]
    public async Task<ActionResult<ServiceResponse<ProductResponseDto>>> DeleteVariant([Required] int attributeId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

		string? userId = GetUserId();

		if (userId is null)
			return BadRequest(ModelState);

		var response = await _variantService.DeleteVariant(attributeId, userId);

        if (response.Success)
            return Ok(response);

        return BadRequest(response.Message);
    }
}
