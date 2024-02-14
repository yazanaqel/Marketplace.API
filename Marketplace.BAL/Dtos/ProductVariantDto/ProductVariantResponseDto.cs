namespace Marketplace.BAL.Dtos.ProductVariantDto;
public class ProductVariantResponseDto(ProductVariant productVariant)
{
    public int VariantId { get; set; } = productVariant.VariantId;
    public string VariantName { get; set; } = productVariant.VariantName;
    public int VariantPrice { get; set; } = productVariant.VariantPrice;
    public int AttributeId { get; set; } = productVariant.AttributeId;
    public string[]? VariantImages { get; set; } = productVariant.VariantImages;
}
