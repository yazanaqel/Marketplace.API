using Marketplace.DAL.Models;

namespace Marketplace.BAL.Services.AttributeService;
public class AttributeService(ApplicationDbContext dbContext, IProductService productService) : IAttributeService
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IProductService _productService = productService;


    public async Task<ServiceResponse<ProductResponseDto>> CreateAttribute(CreateSingleAttributeDto model, string userId)
    {
        ServiceResponse<ProductResponseDto> serviceResponse = new ServiceResponse<ProductResponseDto>();

        try
        {
            var product = await _dbContext.Products.FindAsync(model.ProductId);

            if (product is null || product.UserId != userId)
            {
                serviceResponse.Message = CustomConstants.NotFound.Product;
                serviceResponse.StatusCode = StatusCodes.Status404NotFound;
                return serviceResponse;
            }

            if (!string.IsNullOrWhiteSpace(model.AttributeName))
            {
                var newAttribute = new ProductAttribute
                {
                    ProductId = model.ProductId,
                    AttributeName = model.AttributeName
                };

                if (model is { ProductVariants: not null, ProductVariants.Count: > 0 })
                {
                    newAttribute.ProductVariants = model.ProductVariants
                    .Select(variant => new ProductVariant
                    {
                        VariantName = variant.VariantName,
                        VariantPrice = variant.VariantPrice,
                        AttributeId = newAttribute.AttributeId
                    })
                    .ToList();
                }

                await _dbContext.ProductAttributes.AddAsync(newAttribute);
                await _dbContext.SaveChangesAsync();
            }


        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return await _productService.GetUserProductById(model.ProductId, userId);

    }
    public async Task<ServiceResponse<ProductResponseDto>> DeleteAttribute(int attributeId, string userId)
    {
        ServiceResponse<ProductResponseDto> serviceResponse = new ServiceResponse<ProductResponseDto>();
        int productId = 0;
        try
        {
            var attribute = await _dbContext.ProductAttributes
                .Include(attribute => attribute.Product)
                .Where(attribute => (attribute.AttributeId == attributeId) && (attribute.Product.UserId == userId))
                .FirstOrDefaultAsync();

            if (attribute is null)
            {
                serviceResponse.Message = CustomConstants.NotFound.Attribute;
                serviceResponse.StatusCode = StatusCodes.Status404NotFound;
                return serviceResponse;
            }

            productId = attribute.Product.ProductId;

            _dbContext.ProductAttributes.Remove(attribute);

            await dbContext.SaveChangesAsync();

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return await _productService.GetUserProductById(productId, userId);
    }


}
