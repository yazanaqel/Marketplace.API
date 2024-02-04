using Marketplace.DAL;
using Marketplace.DAL.Dtos.AttributeDto;
using Marketplace.DAL.Dtos.ProductDtos;

namespace Marketplace.BAL.Services.AttributeService;
public interface IAttributeService
{
    Task<ServiceResponse<ProductResponseDto>> AddAttributes(AddAttributeDto model, string userId);
    Task<ServiceResponse<ProductResponseDto>> UpdateAttribute(UpdateAttributeDto model, string userId);
    Task<ServiceResponse<List<ProductResponseDto>>> DeleteAttribute(int attributeId, string userId);
}