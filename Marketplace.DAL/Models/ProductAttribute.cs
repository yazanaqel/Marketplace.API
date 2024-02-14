using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marketplace.DAL.Models;
public class ProductAttribute
{
    [Key]
    public int AttributeId { get; set; }
    public required string AttributeName { get; set; } = string.Empty;

    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; }
    public int ProductId { get; set; }
    public virtual ICollection<ProductVariant>? ProductVariants { get; set; }
}
