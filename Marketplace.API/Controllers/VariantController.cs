using Marketplace.BAL.Services;

namespace Marketplace.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class VariantController(IVariantService variantService) : BaseController
{
    private readonly IVariantService _variantService = variantService;

    [HttpDelete("DeleteVariant")]
    public async Task<ActionResult<ServiceResponse<ProductsResponseDto>>> DeleteVariant([Required] int attributeId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        string? userId = GetUserId();

        if (string.IsNullOrWhiteSpace(userId))
            return BadRequest(ModelState);

        var response = await _variantService.DeleteVariant(attributeId, userId);

        return response.Success ? Ok(response) : BadRequest(response.Message);
    }
}
