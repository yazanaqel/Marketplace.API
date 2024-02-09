using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.DAL.Dtos.ProductVariantDto;
public class UpdateProductVariantDto
{
    public int VariantId { get; set; }

    [MaxLength(15)]
    public string VariantName { get; set; } = "Variant Without Name";
    public string[]? VariantImages { get; set; }
}
