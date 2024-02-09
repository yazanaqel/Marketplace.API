using Marketplace.DAL.Dtos.ProductVariantDto;
using System.ComponentModel.DataAnnotations;

namespace Marketplace.DAL.Dtos.AttributeDto;
public class CreateAttributeDto
{
    [Required,MaxLength(15)]
    public required string AttributeName { get; set; }
    public ICollection<CreateProductVariantDto>? VariantsList { get; set; }
}
