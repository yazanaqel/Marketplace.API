using System.ComponentModel.DataAnnotations;

namespace Marketplace.BAL.Dtos.AttributeDto;
public class CreateAttributeDto
{
    [Required,MaxLength(15)]
    public required string AttributeName { get; set; }
    public ICollection<CreateProductVariantDto>? ProductVariants { get; set; }
}
public class CreateSingleAttributeDto
{
    [Required]
    public required int ProductId { get; set; }
    [Required, MaxLength(15)]
    public required string AttributeName { get; set; }
    public ICollection<CreateProductVariantDto>? ProductVariants { get; set; }
}
