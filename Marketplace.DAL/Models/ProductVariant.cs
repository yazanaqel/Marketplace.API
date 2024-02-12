using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Marketplace.DAL.Models;
public class ProductVariant
{
    [Key]
    public int VariantId { get; set; }
    public string VariantName { get; set; } = string.Empty;
    public string[]? VariantImages { get; set; }

    [ForeignKey(nameof(AttributeId))]
    public virtual ProductAttribute? Attribute { get; set; }
    public int AttributeId { get; set; }
}
