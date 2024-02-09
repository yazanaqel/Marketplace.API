namespace Marketplace.BAL.Services.AttributeService;
public class AttributeService(ApplicationDbContext dbContext, IProductService productService) : IAttributeService
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IProductService _productService = productService;

    public async Task<ServiceResponse<List<ProductsResponseDto>>> DeleteAttribute(int attributeId, string userId)
    {
        ServiceResponse<List<ProductsResponseDto>> serviceResponse = new ServiceResponse<List<ProductsResponseDto>>();

        try
        {
            var attribute = await _dbContext.ProductAttributes
                .Include(attribute => attribute.Product)
                .Where(attribute => attribute.AttributeId == attributeId && attribute.Product.UserId == userId)
                .FirstOrDefaultAsync();

            if (attribute is null)
            {
                serviceResponse.Message = CustomConstants.NotFound.Attribute;
                return serviceResponse;
            }

            _dbContext.ProductAttributes.Remove(attribute);

            await dbContext.SaveChangesAsync();

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return await _productService.GetAllUserProducts(userId, null, null, null, 1, 5);
    }


}
