using Marketplace.DAL.Models;

namespace Marketplace.BAL.Dtos.ProductDtos;
public class ProductResponseDto(Product product)
{
    public int ProductId { get; set; } = product.ProductId;
    public string ProductName { get; set; } = product.ProductName;
    public decimal Price { get; set; } = product.Price;
    public string? ProductMainImage { get; set; } = product.ProductMainImage;
    public string? Description { get; set; } = product.Description;
    public List<string>? ProductImages { get; set; }
    public List<AttributeResponseDto>? ProductAttributesList { get; set; }
        = product.ProductAttributes?.Select(attribute => new AttributeResponseDto(attribute)).ToList();
}