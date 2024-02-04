using Marketplace.DAL.Dtos.AttributeDto;
using Marketplace.DAL.Dtos.ProductDtos;
using Marketplace.DAL;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Marketplace.DAL.Dtos.ProductVariantDto;
using Marketplace.DAL.Models;
using Marketplace.BAL.Services.ProductVariantService;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace Marketplace.API.Controllers;

[Authorize]
public class ProductVariantController(IProductVariantService productVariantService) : Controller
{
    private readonly IProductVariantService _productVariantService = productVariantService;

    [HttpPost("AddProductVariant")]
    public async Task<ActionResult<ServiceResponse<ProductResponseDto>>> AddProductVariant(AddProductVariantDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
            return BadRequest(ModelState);

        var response = await _productVariantService.AddProductVariant(model, userId);

        if (response.Success)
            return Ok(response);

        return BadRequest(response.Message);
    }


    [HttpPut("UpdateProductVariant")]
    public async Task<ActionResult<ServiceResponse<ProductResponseDto>>> UpdateProductVariant(UpdateProductVariantDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
            return BadRequest(ModelState);

        var response = await _productVariantService.UpdateProductVariant(model, userId);

        if (response.Success)
            return Ok(response);

        return BadRequest(response.Message);
    }


    [HttpDelete("DeleteProductVariant")]
    public async Task<ActionResult<ServiceResponse<ProductResponseDto>>> DeleteProductVariant([Required] int attributeId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
            return BadRequest(ModelState);

        var response = await _productVariantService.DeleteProductVariant(attributeId, userId);

        if (response.Success)
            return Ok(response);

        return BadRequest(response.Message);
    }
}
