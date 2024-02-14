namespace Marketplace.BAL.Services.ProductVariantService;
public interface IVariantService
{
    Task <ServiceResponse<IReadOnlyList<ProductsResponseDto>>> DeleteVariant(int variantId, string userId);
}