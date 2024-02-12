using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.BAL.Dtos.ProductVariantDto;
public class ProductVariantResponseDto(ProductVariant productVariant)
{
    public int VariantId { get; set; } = productVariant.VariantId;
    public string VariantName { get; set; } = productVariant.VariantName;
    public int AttributeId { get; set; } = productVariant.AttributeId;
    public string[]? VariantImages { get; set; } = productVariant.VariantImages;
}
