using Marketplace.BAL.MediatR.Queries.ProductQueries;
using Marketplace.BAL.Services;
using MediatR;

namespace Marketplace.BAL.MediatR.Handlers.ProductHandlers;
public class GetProductDetailsHandler(IProductService productService) : IRequestHandler<GetProductDetailsQuery, ServiceResponse<ProductResponseDto>>
{
    private readonly IProductService _productService = productService;

    public async Task<ServiceResponse<ProductResponseDto>> Handle(GetProductDetailsQuery request, CancellationToken cancellationToken)
=> await _productService.GetProductDetails(request.productId);

}
