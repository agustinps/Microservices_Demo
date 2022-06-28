using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ms.shop.domain.Entities;

namespace ms.shop.infrastructure.Data.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(p => p.Amount)
                .HasPrecision(10, 3)
                .IsRequired();
            
            builder.Property(p => p.Price)
                .HasPrecision(10, 3)
                .IsRequired();
            
            builder.Property(p => p.Product)
                .IsRequired()
                .HasMaxLength(150);
            
            builder.Property(p => p.OrderDate)
                .HasDefaultValueSql("GetDate()");

            //builder.HasData(
            //    new Order { Id = 1, Price = 10, Product = "Producto de prueba", Quantity= 1, Amount = 10} 
            //    );
        }
    }
}
