using Marketplace.BAL.Services;

namespace Marketplace.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductController(IProductService productService, IImageService imageService) : BaseController
{
    private readonly IProductService _productService = productService;

    [HttpGet("GetAllUserProducts")]
    public async Task<ActionResult<ServiceResponse<ProductsResponseDto>>> GetAllUserProducts(string? sortColumn, string? sortOrder, string? searchItem, int page = 1, int pageSize = 5)
    {
        string? userId = GetUserId();

        if (string.IsNullOrWhiteSpace(userId))
            return BadRequest(ModelState);

        var response = await _productService.GetAllUserProducts(userId, sortColumn, sortOrder, searchItem, page, pageSize);

        return response.Success ? Ok(response) : NotFound(response.Message);
    }

    [HttpGet("GetUserProductById")]
    public async Task<ActionResult<ServiceResponse<ProductResponseDto>>> GetUserProductById([Required] int productId)
    {
        string? userId = GetUserId();

        if (string.IsNullOrWhiteSpace(userId))
            return BadRequest(ModelState);

        var response = await _productService.GetUserProductById(productId, userId);

        return response.Success ? Ok(response) : NotFound(response.Message);
    }

    [HttpPost("CreateProduct")]
    public async Task<ActionResult<ServiceResponse<ProductResponseDto>>> CreateProduct([FromBody] CreateProductDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        string? userId = GetUserId();

        if (string.IsNullOrWhiteSpace(userId))
            return BadRequest(ModelState);

        var response = await _productService.CreateProduct(model,userId);

        return response.Success ? Created(response.Message, response) : BadRequest(response.Message);
    }


    [HttpPut("UpdateProduct")]
    public async Task<ActionResult<ServiceResponse<ProductResponseDto>>> UpdateProduct([FromBody] UpdateProductDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        string? userId = GetUserId();

        if (string.IsNullOrWhiteSpace(userId))
            return BadRequest(ModelState);

        var response = await _productService.UpdateProduct(model,userId);

        return response.Success ? Ok(response) : NotFound(response.Message);
    }




    [HttpDelete("DeleteProduct")]
    public async Task<ActionResult<ServiceResponse<ProductsResponseDto>>> DeleteProduct([Required] int productId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        string? userId = GetUserId();

        if (string.IsNullOrWhiteSpace(userId))
            return BadRequest(ModelState);

        var response = await _productService.DeleteProduct(productId, userId);

        return response.Success ? NoContent() : NotFound(response.Message);
    }

}
