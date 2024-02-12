using Marketplace.DAL.Models;

namespace Marketplace.BAL.Dtos.AttributeDto;
public class AttributeResponseDto(ProductAttribute productAttribute)
{
    public int AttributeId { get; set; } = productAttribute.AttributeId;
    public string AttributeName { get; set; } = productAttribute.AttributeName;
    public int ProductId { get; set; } = productAttribute.ProductId;
    public List<ProductVariantResponseDto>? ProductVariantList { get; set; }
            = productAttribute.ProductVariants?.Select(variant => new ProductVariantResponseDto(variant)).ToList();
}
