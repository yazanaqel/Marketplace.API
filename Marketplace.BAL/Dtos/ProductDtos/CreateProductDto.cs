using System.ComponentModel.DataAnnotations;

namespace Marketplace.BAL.Dtos.ProductDtos;
public class CreateProductDto
{
    [Required, MaxLength(15)]
    public required string ProductName { get; set; }

    [Required]
    public required decimal Price { get; set; }
    [MaxLength(50)]
    public string? Description { get; set; }
    public IFormFile[]? Images { get; set; }
    public ICollection<CreateAttributeDto>? ProductAttributes { get; set; }

}
