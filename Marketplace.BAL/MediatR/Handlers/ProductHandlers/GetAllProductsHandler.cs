using Marketplace.BAL.MediatR.Queries.ProductQueries;
using Marketplace.BAL.Services;
using MediatR;

namespace Marketplace.BAL.MediatR.Handlers.ProductHandlers;
public class GetAllProductsHandler(IProductService productService) : IRequestHandler<GetAllProductsQuery, ServiceResponse<IReadOnlyList<ProductsResponseDto>>>
{
    private readonly IProductService _productService = productService;

    public async Task<ServiceResponse<IReadOnlyList<ProductsResponseDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    => await _productService.GetAllProducts(request.sortColumn, request.sortOrder, request.searchItem, request.page, request.pageSize);

}
