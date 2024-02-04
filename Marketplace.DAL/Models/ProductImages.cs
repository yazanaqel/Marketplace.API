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
    public ProductImages() => this.ImageId = Guid.NewGuid();
    [Key]
    public Guid ImageId { get; set; }
    public string OriginalFileName { get; set; }
    public string OriginalType { get; set; }
    public byte[] OriginalContent { get; set; }
    public byte[] ThumbnailContent { get; set; }


    [ForeignKey(nameof(ProductId))]
    public Product? Product { get; set; }
    public required int ProductId { get; set; }

}
