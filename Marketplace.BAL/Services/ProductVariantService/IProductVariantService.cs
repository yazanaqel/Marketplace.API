using Marketplace.DAL;
using Marketplace.DAL.Dtos.AttributeDto;
using Marketplace.DAL.Dtos.ProductDtos;
using Marketplace.DAL.Dtos.ProductVariantDto;

namespace Marketplace.BAL.Services.ProductVariantService;
public interface IProductVariantService
{
    Task<ServiceResponse<ProductResponseDto>> AddProductVariant(AddProductVariantDto model, string userId);
    Task<ServiceResponse<ProductResponseDto>> UpdateProductVariant(UpdateProductVariantDto model, string userId);
    Task<ServiceResponse<List<ProductResponseDto>>> DeleteProductVariant(int variantId, string userId);
}