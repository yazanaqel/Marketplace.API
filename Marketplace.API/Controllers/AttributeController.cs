﻿using Marketplace.BAL.Services;

namespace Marketplace.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class AttributeController(IAttributeService attributeService) : BaseController
{
    private readonly IAttributeService _attributeService = attributeService;

    [HttpDelete("DeleteAttribute")]
    public async Task<ActionResult<ServiceResponse<ProductResponseDto>>> DeleteAttribute([Required] int attributeId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

		string? userId = GetUserId();

		if (userId is null)
			return BadRequest(ModelState);

		var response = await _attributeService.DeleteAttribute(attributeId, userId);

        if (response.Success)
            return Ok(response);

        return BadRequest(response.Message);
    }


}
