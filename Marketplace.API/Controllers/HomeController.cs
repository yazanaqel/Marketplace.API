using Marketplace.BAL.MediatR.Queries.ProductQueries;
using Marketplace.BAL.Services;
using MediatR;

namespace Marketplace.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HomeController(IMediator mediator) : Controller
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("GetAllProducts")]
    public async Task<ActionResult<ServiceResponse<ProductsResponseDto>>> GetAllProducts(string? sortColumn, string? sortOrder, string? searchItem, int page = 1, int pageSize = 5)
    {
        var response = await _mediator.Send(new GetAllProductsQuery(sortColumn, sortOrder, searchItem, page, pageSize));

        return response.Success ? Ok(response) : NotFound(response.Message);
    }



    [HttpGet("GetProductDetails")]
    public async Task<ActionResult<ServiceResponse<ProductResponseDto>>> GetProductDetails([Required] int productId)
    {
        var response = await _mediator.Send(new GetProductDetailsQuery(productId));

        return response.Success ? Ok(response) : NotFound(response.Message);
    }
}
