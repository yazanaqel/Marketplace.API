using System.ComponentModel.DataAnnotations;

namespace Marketplace.BAL.Dtos.ProductVariantDto;
public class CreateProductVariantDto
{
    [Required, MaxLength(15)]
    public required string VariantName { get; set; }
    public int VariantPrice { get; set; }
    public string[]? VariantImages { get; set; }

}
