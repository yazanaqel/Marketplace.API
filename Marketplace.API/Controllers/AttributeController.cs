using Marketplace.BAL.Services.AttributeService;
using Marketplace.DAL.Dtos.ProductDtos;
using Marketplace.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Marketplace.DAL.Dtos.AttributeDto;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace Marketplace.API.Controllers;
[Route("api/[controller]")]
[ApiController]

[Authorize]
public class AttributeController(IAttributeService attributeService) : ControllerBase
{
    private readonly IAttributeService _attributeService = attributeService;


    [HttpPost("AddAttributes")]
    public async Task<ActionResult<ServiceResponse<ProductResponseDto>>> AddAttributes(AddAttributeDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
            return BadRequest(ModelState);

        var response = await _attributeService.AddAttributes(model, userId);

        if (response.Success)
            return Ok(response);

        return BadRequest(response.Message);
    }


    [HttpPut("UpdateAttribute")]
    public async Task<ActionResult<ServiceResponse<ProductResponseDto>>> UpdateAttribute(UpdateAttributeDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
            return BadRequest(ModelState);

        var response = await _attributeService.UpdateAttribute(model, userId);

        if (response.Success)
            return Ok(response);

        return BadRequest(response.Message);
    }


    [HttpDelete("DeleteAttribute")]
    public async Task<ActionResult<ServiceResponse<ProductResponseDto>>> DeleteAttribute([Required] int attributeId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
            return BadRequest(ModelState);

        var response = await _attributeService.DeleteAttribute(attributeId, userId);

        if (response.Success)
            return Ok(response);

        return BadRequest(response.Message);
    }


}
