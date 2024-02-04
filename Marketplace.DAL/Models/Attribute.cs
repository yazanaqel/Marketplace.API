using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.DAL.Models;
public class Attribute
{
    [Key]
    public int AttributeId { get; set; }
    public required string AttributeName { get; set; } = string.Empty;

    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; }
    public required int ProductId { get; set; }
    public virtual List<ProductVariant>? ProductVariants { get; set; }
}
