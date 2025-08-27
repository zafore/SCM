using Microsoft.EntityFrameworkCore;
using Suppliers.Api.Models;

namespace Suppliers.Api.Data
{
    public class SuppliersDbContext : DbContext
    {
        public SuppliersDbContext(DbContextOptions<SuppliersDbContext> options) : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<AttachmentFile> AttachmentFiles { get; set; }
        public DbSet<Carrier> Carriers { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<InstallmentsType> InstallmentsTypes { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<OfferContract> OfferContracts { get; set; }
        public DbSet<OfferItem> OfferItems { get; set; }
        public DbSet<OfferShippingContract> OfferShippingContracts { get; set; }
        public DbSet<OfferShippingCost> OfferShippingCosts { get; set; }
        public DbSet<OfferStatus> OfferStatuses { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<PaymentState> PaymentStates { get; set; }
        public DbSet<RoleType> RoleTypes { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ShipmentType> ShipmentTypes { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierContact> SupplierContacts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and constraints
            ConfigureRelationships(modelBuilder);
        }

        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            // Attachment relationships
            modelBuilder.Entity<AttachmentFile>()
                .HasOne(af => af.Attachment)
                .WithMany(a => a.AttachmentFiles)
                .HasForeignKey(af => af.AttachmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Offer relationships
            modelBuilder.Entity<Offer>()
                .HasOne(o => o.OfferStatus)
                .WithMany(os => os.Offers)
                .HasForeignKey(o => o.OfferStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Offer>()
                .HasOne(o => o.Supplier)
                .WithMany(s => s.Offers)
                .HasForeignKey(o => o.SupplierID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Offer>()
                .HasOne(o => o.Attachment)
                .WithMany(a => a.Offers)
                .HasForeignKey(o => o.AttachmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // OfferContract relationships
            modelBuilder.Entity<OfferContract>()
                .HasOne(oc => oc.Offer)
                .WithMany(o => o.OfferContracts)
                .HasForeignKey(oc => oc.OfferID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OfferContract>()
                .HasOne(oc => oc.InstallmentsType)
                .WithMany(it => it.OfferContracts)
                .HasForeignKey(oc => oc.InstallmentsTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OfferContract>()
                .HasOne(oc => oc.Currency)
                .WithMany(c => c.OfferContracts)
                .HasForeignKey(oc => oc.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OfferContract>()
                .HasOne(oc => oc.Attachment)
                .WithMany(a => a.OfferContracts)
                .HasForeignKey(oc => oc.AttachmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OfferContract>()
                .HasOne(oc => oc.PaymentState)
                .WithMany(ps => ps.OfferContracts)
                .HasForeignKey(oc => oc.PaymentStatesId)
                .OnDelete(DeleteBehavior.Restrict);

            // OfferItem relationships
            modelBuilder.Entity<OfferItem>()
                .HasOne(oi => oi.Offer)
                .WithMany(o => o.OfferItems)
                .HasForeignKey(oi => oi.OfferID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OfferItem>()
                .HasOne(oi => oi.Item)
                .WithMany(i => i.OfferItems)
                .HasForeignKey(oi => oi.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OfferItem>()
                .HasOne(oi => oi.Currency)
                .WithMany(c => c.OfferItems)
                .HasForeignKey(oi => oi.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OfferItem>()
                .HasOne(oi => oi.PrmotionFileAttachment)
                .WithMany(a => a.OfferItems)
                .HasForeignKey(oi => oi.PrmotionFileAttachmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // OfferShippingCost relationships
            modelBuilder.Entity<OfferShippingCost>()
                .HasOne(osc => osc.Carrier)
                .WithMany(c => c.OfferShippingCosts)
                .HasForeignKey(osc => osc.CarrierID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OfferShippingCost>()
                .HasOne(osc => osc.ShipmentType)
                .WithMany(st => st.OfferShippingCosts)
                .HasForeignKey(osc => osc.ShipmentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OfferShippingCost>()
                .HasOne(osc => osc.Currency)
                .WithMany(c => c.OfferShippingCosts)
                .HasForeignKey(osc => osc.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OfferShippingCost>()
                .HasOne(osc => osc.Attachment)
                .WithMany(a => a.OfferShippingCosts)
                .HasForeignKey(osc => osc.AttachmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OfferShippingCost>()
                .HasOne(osc => osc.Offer)
                .WithMany(o => o.OfferShippingCosts)
                .HasForeignKey(osc => osc.OfferID)
                .OnDelete(DeleteBehavior.Cascade);

            // OfferShippingContract relationships
            modelBuilder.Entity<OfferShippingContract>()
                .HasOne(osc => osc.OfferShippingCost)
                .WithMany(osc => osc.OfferShippingContracts)
                .HasForeignKey(osc => osc.OfferShippingCostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OfferShippingContract>()
                .HasOne(osc => osc.InstallmentsType)
                .WithMany(it => it.OfferShippingContracts)
                .HasForeignKey(osc => osc.InstallmentsTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OfferShippingContract>()
                .HasOne(osc => osc.Currency)
                .WithMany(c => c.OfferShippingContracts)
                .HasForeignKey(osc => osc.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OfferShippingContract>()
                .HasOne(osc => osc.Attachment)
                .WithMany(a => a.OfferShippingContracts)
                .HasForeignKey(osc => osc.AttachmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OfferShippingContract>()
                .HasOne(osc => osc.PaymentState)
                .WithMany(ps => ps.OfferShippingContracts)
                .HasForeignKey(osc => osc.PaymentStatesId)
                .OnDelete(DeleteBehavior.Restrict);

            // Supplier relationships
            modelBuilder.Entity<Supplier>()
                .HasOne(s => s.Country)
                .WithMany(c => c.Suppliers)
                .HasForeignKey(s => s.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Supplier>()
                .HasOne(s => s.Attachment)
                .WithMany(a => a.Suppliers)
                .HasForeignKey(s => s.AttachmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SupplierContact>()
                .HasOne(sc => sc.Supplier)
                .WithMany(s => s.SupplierContacts)
                .HasForeignKey(sc => sc.SupplierId)
                .OnDelete(DeleteBehavior.Cascade);

            // Role relationships
            modelBuilder.Entity<Role>()
                .HasOne(r => r.RoleType)
                .WithMany(rt => rt.Roles)
                .HasForeignKey(r => r.RoleTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
