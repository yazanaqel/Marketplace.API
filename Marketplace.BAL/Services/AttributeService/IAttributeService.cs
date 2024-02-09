namespace Marketplace.BAL.Services.AttributeService;
public interface IAttributeService
{
    Task<ServiceResponse<List<ProductsResponseDto>>> DeleteAttribute(int attributeId, string userId);
}