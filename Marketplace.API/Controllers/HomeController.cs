using Marketplace.DAL.Dtos.ProductDtos;
using Marketplace.DAL;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Marketplace.BAL.Services.ImageService;
using Marketplace.BAL.Services.ProductService;

namespace Marketplace.API.Controllers;
public class HomeController(IProductService productService, IImageService imageService) : Controller
{
    private readonly IProductService _productService = productService;
    private readonly IImageService _imageService = imageService;

    [HttpGet("GetAllProducts")]
    public async Task<ActionResult<ServiceResponse<ProductResponseDto>>> GetAllProducts(string? sortColumn, string? sortOrder, string? searchItem, int page = 1, int pageSize = 5)
    {

        var response = await _productService.GetAllProducts(sortColumn, sortOrder, searchItem, page, pageSize);

        if (response.Success)
            return Ok(response);

        return BadRequest(response.Message);
    }

    [HttpGet("GetThumbnail")] // insert the image id here to display it
    public async Task<IActionResult> GetThumbnail([Required] string id)
    {
        var image = File(await _imageService.GetThumbnail(id), "image/jpeg");

        if (image is null)
            return NotFound();

        return image;
    }
}
