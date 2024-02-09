using Marketplace.DAL.Dtos.ProductVariantDto;
using System.ComponentModel.DataAnnotations;

namespace Marketplace.DAL.Dtos.AttributeDto;
public class UpdateAttributeDto
{
    public int AttributeId { get; set; }

    [MaxLength(15)]
    public string AttributeName { get; set; } = "Attribute Without Name";
    public ICollection<UpdateProductVariantDto>? VariantsList { get; set; }
}
