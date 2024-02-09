using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Marketplace.BAL.DbContext;
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<ProductAttribute> ProductAttributes { get; set; }
    public DbSet<ProductVariant> ProductVariants { get; set; }
    public DbSet<ProductImages> ProductImages { get; set; }
   
}
