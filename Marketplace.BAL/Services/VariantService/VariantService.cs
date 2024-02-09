namespace Marketplace.BAL.Services.ProductVariantService;
public class VariantService(ApplicationDbContext dbContext, IProductService productService) : IVariantService
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IProductService _productService = productService;

    public async Task<ServiceResponse<List<ProductsResponseDto>>> DeleteVariant(int variantId, string userId)
    {
        ServiceResponse<List<ProductsResponseDto>> serviceResponse = new ServiceResponse<List<ProductsResponseDto>>();

        try
        {
            var variant = await _dbContext.ProductVariants
                .Include(variant => variant.Attribute)
                .ThenInclude(product => product.Product)
                .Where(variant => variant.VariantId.Equals(variantId) && variant.Attribute.Product.UserId.Equals(userId))
                .FirstOrDefaultAsync();

            if (variant is null)
            {
                serviceResponse.Message = CustomConstants.NotFound.Variant;
                return serviceResponse;
            }

            _dbContext.ProductVariants.Remove(variant);

            await dbContext.SaveChangesAsync();

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return await _productService.GetAllUserProducts(userId, null, null, null, 1, 5);
    }

}
