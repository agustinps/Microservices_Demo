using Microsoft.EntityFrameworkCore;
using Product.Service.Models;

namespace Product.Service.Infrastructure.Data.EntityFramework;

internal class ProductContext : DbContext
{
    public ProductContext(DbContextOptions<ProductContext> options)
        : base(options)
    {
    }

    public DbSet<Models.Product> Products { get; set; }
    public DbSet<ProductType> ProductTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }


}
