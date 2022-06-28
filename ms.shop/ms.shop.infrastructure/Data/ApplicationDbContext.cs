using Microsoft.EntityFrameworkCore;
using ms.shop.domain.Entities;
using System.Reflection;

namespace ms.shop.infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {            
            configurationBuilder.Properties<DateTime>().HaveColumnType("Date");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Order> Orders { get; set; }
    }
}
