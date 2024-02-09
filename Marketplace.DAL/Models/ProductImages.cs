using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.DAL.Models;
public class ProductImages
{
    public Guid Id { get; set; }
    public required string Folder { get; set; }


    [ForeignKey(nameof(ProductId))]
    public Product? Product { get; set; }
    public required int ProductId { get; set; }

}
