using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Marketplace.DAL.Dtos.ProductDtos;
public class AddProductDto
{
    [Required,MaxLength(15)]
    public required string ProductName { get; set; }

    [Required]
    public required decimal Price { get; set; }
    [MaxLength(50)]
    public string? Description { get; set; }
    public IFormFile[]? Images { get; set; }

}
