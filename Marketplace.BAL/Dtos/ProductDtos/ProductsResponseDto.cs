using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.DAL.Dtos.ProductDtos;
public class ProductsResponseDto
{
    public required int ProductId { get; set; }
    public required string ProductName { get; set; }
    public required decimal Price { get; set; }
    public string? ProductMainImage { get; set; }
    public string? Description { get; set; }
}
