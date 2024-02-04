using Marketplace.DAL.Dtos.ProductVariantDto;

namespace Marketplace.DAL.Dtos.AttributeDto;
public class AttributeResponseDto
{
    public required int AttributeId { get; set; }
    public required string AttributeName { get; set; } = string.Empty;
    public required int ProductId { get; set; }
    public List<ProductVariantResponseDto>? ProductVariantList { get; set; }
}
