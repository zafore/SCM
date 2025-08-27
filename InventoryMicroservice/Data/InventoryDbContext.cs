using Microsoft.EntityFrameworkCore;
using InventoryMicroservice.Models;

namespace InventoryMicroservice.Data
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<InventoryMovement> InventoryMovements { get; set; }
        public DbSet<StockAlert> StockAlerts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and constraints
            ConfigureRelationships(modelBuilder);
            ConfigureIndexes(modelBuilder);
        }

        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            // Warehouse relationships
            modelBuilder.Entity<InventoryItem>()
                .HasOne(ii => ii.Warehouse)
                .WithMany(w => w.InventoryItems)
                .HasForeignKey(ii => ii.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            // InventoryItem relationships
            modelBuilder.Entity<InventoryMovement>()
                .HasOne(im => im.InventoryItem)
                .WithMany(ii => ii.InventoryMovements)
                .HasForeignKey(im => im.InventoryItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // StockAlert relationships
            modelBuilder.Entity<StockAlert>()
                .HasOne(sa => sa.InventoryItem)
                .WithMany()
                .HasForeignKey(sa => sa.InventoryItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure enums
            modelBuilder.Entity<InventoryMovement>()
                .Property(im => im.MovementType)
                .HasConversion<int>();

            modelBuilder.Entity<StockAlert>()
                .Property(sa => sa.AlertType)
                .HasConversion<int>();
        }

        private void ConfigureIndexes(ModelBuilder modelBuilder)
        {
            // Indexes for better performance
            modelBuilder.Entity<InventoryItem>()
                .HasIndex(ii => new { ii.ItemId, ii.WarehouseId })
                .IsUnique();

            modelBuilder.Entity<InventoryMovement>()
                .HasIndex(im => im.MovementDate);

            modelBuilder.Entity<InventoryMovement>()
                .HasIndex(im => im.Reference);

            modelBuilder.Entity<StockAlert>()
                .HasIndex(sa => new { sa.IsRead, sa.AlertDate });

            modelBuilder.Entity<Warehouse>()
                .HasIndex(w => w.WarehouseName);
        }
    }
}
