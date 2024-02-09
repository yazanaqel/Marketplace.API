using Marketplace.BAL.Services;

namespace Marketplace.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductController(IProductService productService, IImageService imageService) : BaseController
{
    private readonly IProductService _productService = productService;

    [HttpGet("GetAllUserProducts")]
    public async Task<ActionResult<ServiceResponse<ProductResponseDto>>> GetAllUserProducts(string? sortColumn, string? sortOrder, string? searchItem, int page = 1, int pageSize = 5)
    {
        string? userId = GetUserId();

        if (userId is null)
            return BadRequest(ModelState);

        var response = await _productService.GetAllUserProducts(userId, sortColumn, sortOrder, searchItem, page, pageSize);

        if (response.Success)
            return Ok(response);

        return NotFound(response.Message);
    }

    [HttpGet("GetUserProductById")]
    public async Task<ActionResult<ServiceResponse<ProductResponseDto>>> GetUserProductById([Required] int productId)
    {

        string? userId = GetUserId();

        if (userId is null)
            return BadRequest(ModelState);

        var response = await _productService.GetUserProductById(productId, userId);

        if (response.Success)
            return Ok(response);

        return NotFound(response.Message);
    }

    [HttpPost("CreateProduct")]
    public async Task<ActionResult<ServiceResponse<ProductResponseDto>>> CreateProduct([FromForm] CreateProductDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        string? userId = GetUserId();

        if (userId is null)
            return BadRequest(ModelState);

        var response = await _productService.CreateProduct(model, userId);

        if (response.Success)
            return Created(response.Message, response);

        return BadRequest(response.Message);
    }


    [HttpPut("UpdateProduct")]
    public async Task<ActionResult<ServiceResponse<ProductResponseDto>>> UpdateProduct([FromForm] UpdateProductDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        string? userId = GetUserId();

        if (userId is null)
            return BadRequest(ModelState);

        var response = await _productService.UpdateProduct(model, userId);

        if (response.Success)
            return Ok(response);

        return NotFound(response.Message);
    }




    [HttpDelete("DeleteProduct")]
    public async Task<ActionResult<ServiceResponse<ProductResponseDto>>> DeleteProduct([Required] int productId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        string? userId = GetUserId();

        if (userId is null)
            return BadRequest(ModelState);

        var response = await _productService.DeleteProduct(productId, userId);

        if (response.Success)
            return NoContent();

        return NotFound(response.Message);
    }

}
