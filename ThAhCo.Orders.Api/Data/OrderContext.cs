using Microsoft.EntityFrameworkCore;

namespace ThAhCo.Orders.Api.Data
{
    public class OrderContext : DbContext
    {
        public DbSet<Order> Orders { get; set; } = null!;

        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Order>(x =>
            {
                x.Property(c => c.CustomerId).IsRequired();
                x.Property(c => c.RequestedDate).IsRequired();
            });
        }
    }
}
