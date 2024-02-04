using Marketplace.BAL.Services.ProductService;
using Marketplace.DAL.Dtos.UserDtos;
using Marketplace.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Marketplace.DAL.Dtos.ProductDtos;
using Marketplace.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.Net.Http.Headers;
using Marketplace.BAL.Services.ImageService;
using System.ComponentModel.DataAnnotations;

namespace Marketplace.API.Controllers;
[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class ProductController(IProductService productService, IImageService imageService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    [HttpGet("GetAllUserProducts")]
    public async Task<ActionResult<ServiceResponse<ProductResponseDto>>> GetAllUserProducts(string? sortColumn, string? sortOrder, string? searchItem, int page = 1, int pageSize = 5)
    {
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
            return BadRequest(ModelState);

        var response = await _productService.GetAllUserProducts(userId, sortColumn, sortOrder, searchItem, page, pageSize);

        if (response.Success)
            return Ok(response);

        return BadRequest(response.Message);
    }

    [HttpGet("GetUserProductById")]
    public async Task<ActionResult<ServiceResponse<ProductResponseDto>>> GetUserProductById([Required] int productId)
    {
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
            return BadRequest(ModelState);

        var response = await _productService.GetUserProductById(productId, userId);

        if (response.Success)
            return Ok(response);

        return BadRequest(response.Message);
    }

    [HttpPost("AddProduct")]
    public async Task<ActionResult<ServiceResponse<ProductResponseDto>>> AddProduct(AddProductDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        //string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //if (userId is null)
        //    return BadRequest(ModelState);

        var response = await _productService.AddProduct(model, "01241996-fc2a-41ad-90ed-a37a72cf6d1c");

        if (response.Success)
            return Ok(response);

        return BadRequest(response.Message);
    }


    [HttpPut("UpdateProduct")]
    public async Task<ActionResult<ServiceResponse<ProductResponseDto>>> UpdateProduct(UpdateProductDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
            return BadRequest(ModelState);

        var response = await _productService.UpdateProduct(model, userId);

        if (response.Success)
            return Ok(response);

        return BadRequest(response.Message);
    }




    [HttpDelete("DeleteProduct")]
    public async Task<ActionResult<ServiceResponse<ProductResponseDto>>> DeleteProduct([Required] int productId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
            return BadRequest(ModelState);

        var response = await _productService.DeleteProduct(productId, userId);

        if (response.Success)
            return Ok(response);

        return BadRequest(response.Message);
    }

}
