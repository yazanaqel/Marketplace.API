using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.DAL.Dtos.ProductVariantDto;
public class ProductVariantResponseDto
{
    public required int VariantId { get; set; }
    public required string VariantName { get; set; }
    public required int AttributeId { get; set; }
    public string[]? VariantImages { get; set; }
}
