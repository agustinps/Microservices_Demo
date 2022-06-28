using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ms.storage.domain.Entities;

namespace ms.storage.infrastructure.Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.ProductName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Price)
            .HasPrecision(10, 2)
            .IsRequired();

            builder.Property(p => p.Stock).IsRequired();

            builder.Property(p => p.Supplier).HasMaxLength(100);
        }
    }
}
