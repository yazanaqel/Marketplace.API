using Marketplace.DAL.Dtos.AttributeDto;

namespace Marketplace.DAL.Dtos.ProductDtos;
public class ProductResponseDto
{
    public required int ProductId { get; set; }
    public required string ProductName { get; set; }
    public required decimal Price { get; set; }
    public string? ProductMainImage { get; set; }
    public string? Description { get; set; }
    public List<string>? ProductImages { get; set; }
    public List<AttributeResponseDto>? ProductAttributesList { get; set; }
}