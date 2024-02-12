namespace Marketplace.BAL.Services.ProductVariantService;
public interface IVariantService
{
    Task<ServiceResponse<ProductResponseDto>> DeleteVariant(int variantId, string userId);
}