using Marketplace.BAL.Dtos.AttributeDto;
using Marketplace.BAL.Services;

namespace Marketplace.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class AttributeController(IAttributeService attributeService) : BaseController
{
    private readonly IAttributeService _attributeService = attributeService;

    [HttpPost("CreateAttribute")]
    public async Task<ActionResult<ServiceResponse<ProductResponseDto>>> CreateAttribute([FromBody] CreateSingleAttributeDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        string? userId = GetUserId();

        if (string.IsNullOrWhiteSpace(userId))
            return BadRequest(ModelState);

        var response = await _attributeService.CreateAttribute(model, userId);

        return response.Success ? Created(response.Message, response) : BadRequest(response.Message);
    }

    [HttpDelete("DeleteAttribute")]
    public async Task<ActionResult<ServiceResponse<ProductsResponseDto>>> DeleteAttribute([Required] int attributeId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        string? userId = GetUserId();

        if (string.IsNullOrWhiteSpace(userId))
            return BadRequest(ModelState);

        var response = await _attributeService.DeleteAttribute(attributeId, userId);

        return response.Success ? Ok(response) : BadRequest(response.Message);
    }


}
