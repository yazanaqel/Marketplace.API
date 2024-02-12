﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Marketplace.DAL.Models;
public class Product
{
    [Key]
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? ProductMainImage { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    public string? Description { get; set; }


    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser User { get; set; }
    public string UserId { get; set; } = string.Empty;
    public virtual ICollection<ProductAttribute>? ProductAttributes { get; set; }
    public virtual ICollection<ProductImages>? ProductImages { get; set; }
}