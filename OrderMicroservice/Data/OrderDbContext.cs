using Microsoft.EntityFrameworkCore;
using OrderMicroservice.Models;

namespace OrderMicroservice.Data
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and constraints
            ConfigureRelationships(modelBuilder);
            ConfigureIndexes(modelBuilder);
        }

        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            // Order relationships
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderStatusHistory>()
                .HasOne(osh => osh.Order)
                .WithMany(o => o.OrderStatusHistories)
                .HasForeignKey(osh => osh.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure enums
            modelBuilder.Entity<Order>()
                .Property(o => o.OrderType)
                .HasConversion<int>();

            modelBuilder.Entity<Order>()
                .Property(o => o.OrderStatus)
                .HasConversion<int>();

            modelBuilder.Entity<Order>()
                .Property(o => o.PaymentStatus)
                .HasConversion<int>();

            modelBuilder.Entity<OrderStatusHistory>()
                .Property(osh => osh.PreviousStatus)
                .HasConversion<int>();

            modelBuilder.Entity<OrderStatusHistory>()
                .Property(osh => osh.NewStatus)
                .HasConversion<int>();
        }

        private void ConfigureIndexes(ModelBuilder modelBuilder)
        {
            // Indexes for better performance
            modelBuilder.Entity<Order>()
                .HasIndex(o => o.OrderNumber)
                .IsUnique();

            modelBuilder.Entity<Order>()
                .HasIndex(o => o.CustomerId);

            modelBuilder.Entity<Order>()
                .HasIndex(o => o.SupplierId);

            modelBuilder.Entity<Order>()
                .HasIndex(o => o.OrderDate);

            modelBuilder.Entity<Order>()
                .HasIndex(o => o.OrderStatus);

            modelBuilder.Entity<OrderItem>()
                .HasIndex(oi => oi.ItemId);

            modelBuilder.Entity<OrderStatusHistory>()
                .HasIndex(osh => osh.ChangedDate);
        }
    }
}
