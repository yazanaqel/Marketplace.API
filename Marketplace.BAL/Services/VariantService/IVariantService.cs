namespace Marketplace.BAL.Services.ProductVariantService;
public interface IVariantService
{
    Task<ServiceResponse<List<ProductsResponseDto>>> DeleteVariant(int variantId, string userId);
}