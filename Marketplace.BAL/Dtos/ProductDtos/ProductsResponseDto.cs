namespace Marketplace.BAL.Dtos.ProductDtos;
public class ProductsResponseDto(Product product)
{
    public int ProductId { get; set; } = product.ProductId;
    public string ProductName { get; set; } = product.ProductName;
    public decimal Price { get; set; } = product.Price;
    public string? ProductMainImage { get; set; } = product.ProductMainImage;
    public string? Description { get; set; } = product.Description;
}
