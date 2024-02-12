namespace Marketplace.BAL.Services.AttributeService;
public interface IAttributeService
{
    Task<ServiceResponse<ProductResponseDto>> DeleteAttribute(int attributeId, string userId);
}