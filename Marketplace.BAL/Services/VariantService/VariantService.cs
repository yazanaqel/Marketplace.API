using Marketplace.DAL.Models;

namespace Marketplace.BAL.Services.ProductVariantService;
public class VariantService(ApplicationDbContext dbContext, IProductService productService) : IVariantService
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IProductService _productService = productService;

    public async Task<ServiceResponse<ProductResponseDto>> DeleteVariant(int variantId, string userId)
    {
        ServiceResponse<ProductResponseDto> serviceResponse = new ServiceResponse<ProductResponseDto>();
        int productId = 0;
        try
        {
            IQueryable<ProductVariant> variantQuery = _dbContext.ProductVariants
                .Include(variant => variant.Attribute)
                .ThenInclude(product => product.Product)
                .Where(variant => variant.VariantId == variantId && variant.Attribute.Product.UserId == userId);

            if (!await variantQuery.AnyAsync())
            {
                serviceResponse.Message = CustomConstants.NotFound.Variant;
                return serviceResponse;
            }

            var variant = await variantQuery.FirstOrDefaultAsync();

            productId = variant.Attribute.Product.ProductId;

            _dbContext.ProductVariants.Remove(variant);

            await dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return await _productService.GetUserProductById(productId, userId);
    }

}
