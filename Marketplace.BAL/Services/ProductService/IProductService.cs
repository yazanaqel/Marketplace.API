namespace Marketplace.BAL.Services.ProductService;
public interface IProductService
{
    Task<ServiceResponse<IReadOnlyList<ProductsResponseDto>>> GetAllProducts(string? sortColumn,string? sortOrder, string? searchItem,int page, int pageSize);
    Task<ServiceResponse<ProductResponseDto>> GetProductDetails(int productId);
    Task<ServiceResponse<IReadOnlyList<ProductsResponseDto>>> GetAllUserProducts(string userId, string? sortColumn, string? sortOrder, string? searchItem, int page, int pageSize);
    Task<ServiceResponse<ProductResponseDto>> GetUserProductById(int productId, string userId);
    Task<ServiceResponse<ProductResponseDto>> CreateProduct(CreateProductDto model, string userId);
    Task<ServiceResponse<ProductResponseDto>> UpdateProduct(UpdateProductDto model, string userId);
    Task<ServiceResponse<ProductResponseDto>> DeleteProduct(int productId, string userId);
}