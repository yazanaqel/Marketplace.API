using Marketplace.DAL.Dtos.AttributeDto;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Marketplace.DAL.Dtos.ProductDtos;
public class UpdateProductDto
{
    [Required]
    public required int ProductId { get; set; }

    [MaxLength(15)]
    public string ProductName { get; set; } = "Product Without Name";
    public string? ProductMainImage { get; set; }
    public decimal Price { get; set; }
    [MaxLength(50)]
    public string? Description { get; set; }
	public IFormFile[]? Images { get; set; }
	public ICollection<UpdateAttributeDto>? ProductAttributesList { get; set; }
}
