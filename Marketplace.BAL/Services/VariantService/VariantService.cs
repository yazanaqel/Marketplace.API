using Marketplace.DAL.Models;
using System.Collections.Generic;

namespace Marketplace.BAL.Services.ProductVariantService;
public class VariantService(ApplicationDbContext dbContext, IProductService productService) : IVariantService
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IProductService _productService = productService;

    public async Task<ServiceResponse<IReadOnlyList<ProductsResponseDto>>> DeleteVariant(int variantId, string userId)
    {
        try
        {
            await _dbContext.ProductVariants
                .Where(variant => (variant.VariantId == variantId) && (variant.Attribute.Product.UserId == userId))
                .ExecuteDeleteAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return await _productService.GetAllUserProducts(userId, null, null, null, 1, 5);
    }

}
