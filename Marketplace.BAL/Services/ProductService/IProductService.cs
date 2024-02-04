using Marketplace.DAL;
using Marketplace.DAL.Dtos.ProductDtos;

namespace Marketplace.BAL.Services.ProductService;
public interface IProductService
{
    Task<ServiceResponse<List<ProductResponseDto>>> GetAllProducts(string? sortColumn,string? sortOrder, string? searchItem,int page, int pageSize);
    Task<ServiceResponse<ProductResponseDto>> AddProduct(AddProductDto model, string userId);
    Task<ServiceResponse<List<ProductResponseDto>>> GetAllUserProducts(string userId, string? sortColumn, string? sortOrder, string? searchItem, int page, int pageSize);
    Task<ServiceResponse<ProductResponseDto>> GetUserProductById(int productId, string userId);
    Task<ServiceResponse<ProductResponseDto>> UpdateProduct(UpdateProductDto model, string userId);
    Task<ServiceResponse<List<ProductResponseDto>>> DeleteProduct(int productId, string userId);
}