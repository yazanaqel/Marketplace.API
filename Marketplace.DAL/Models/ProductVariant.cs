using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marketplace.DAL.Models;
public class ProductVariant
{
    [Key]
    public int VariantId { get; set; }
    public required string VariantName { get; set; }
    public int VariantPrice { get; set; }
    public string[]? VariantImages { get; set; }

    [ForeignKey(nameof(AttributeId))]
    public virtual ProductAttribute? Attribute { get; set; }
    public int AttributeId { get; set; }
}
