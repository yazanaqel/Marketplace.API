namespace Marketplace.BAL.Services.AttributeService;
public interface IAttributeService
{
    Task<ServiceResponse<ProductResponseDto>> DeleteAttribute(int attributeId, string userId);
    Task<ServiceResponse<ProductResponseDto>> CreateAttribute(CreateSingleAttributeDto model, string userId);
}