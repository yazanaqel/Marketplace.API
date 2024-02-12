using Marketplace.BAL.Services;

namespace Marketplace.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HomeController(IProductService productService) : Controller
{
    private readonly IProductService _productService = productService;

    [HttpGet("GetAllProducts")]
    public async Task<ActionResult<ServiceResponse<ProductsResponseDto>>> GetAllProducts(string? sortColumn, string? sortOrder, string? searchItem, int page = 1, int pageSize = 5)
    {

        var response = await _productService.GetAllProducts(sortColumn, sortOrder, searchItem, page, pageSize);

        if (response.Success)
            return Ok(response);

        return NotFound(response.Message);
    }


    [HttpGet("GetProductDetails")]
    public async Task<ActionResult<ServiceResponse<ProductResponseDto>>> GetProductDetails([Required] int productId)
    {

        var response = await _productService.GetProductDetails(productId);

        if (response.Success)
            return Ok(response);

        return NotFound(response.Message);
    }
}
