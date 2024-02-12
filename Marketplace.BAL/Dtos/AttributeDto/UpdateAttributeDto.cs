using System.ComponentModel.DataAnnotations;

namespace Marketplace.BAL.Dtos.AttributeDto;
public class UpdateAttributeDto
{
    public int AttributeId { get; set; }

    [Required, MaxLength(15)]
    public required string AttributeName { get; set; }
    public ICollection<UpdateProductVariantDto>? ProductVariants { get; set; }
}
