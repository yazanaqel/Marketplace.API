using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.DAL.Dtos.ProductVariantDto;
public class UpdateProductVariantDto
{
    [Required]
    public required int VariantId { get; set; }

    [Required, MaxLength(15)]
    public required string VariantName { get; set; }
    public string[]? VariantImages { get; set; }
}
