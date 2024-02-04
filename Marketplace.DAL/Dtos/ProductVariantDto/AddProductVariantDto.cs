using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.DAL.Dtos.ProductVariantDto;
public class AddProductVariantDto
{
    [Required, MaxLength(15)]
    public required string VariantName { get; set; }
    public string[]? VariantImages { get; set; }

    [Required]
    public required int AttributeId { get; set; }

}
