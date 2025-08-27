using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace IdentityMicroservice.Entities;

public partial class ScmdbContext : DbContext
{


    public ScmdbContext(DbContextOptions<ScmdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attachment> Attachments { get; set; }

    public virtual DbSet<AttachmentFile> AttachmentFiles { get; set; }

    public virtual DbSet<Carrier> Carriers { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<InstallmentsType> InstallmentsTypes { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Offer> Offers { get; set; }

    public virtual DbSet<OfferContract> OfferContracts { get; set; }

    public virtual DbSet<OfferItem> OfferItems { get; set; }

    public virtual DbSet<OfferShippingContract> OfferShippingContracts { get; set; }

    public virtual DbSet<OfferShippingCost> OfferShippingCosts { get; set; }

    public virtual DbSet<OfferStatus> OfferStatuses { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<PaymentState> PaymentStates { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RoleType> RoleTypes { get; set; }

    public virtual DbSet<ShipmentType> ShipmentTypes { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<SupplierContact> SupplierContacts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConUser");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Arabic_100_BIN");

        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.ToTable("Attachment");

            entity.Property(e => e.AttachmentDate).HasColumnType("datetime");
            entity.Property(e => e.AttachmentForTable).HasMaxLength(50);
            entity.Property(e => e.AttachmentName).HasMaxLength(50);
        });

        modelBuilder.Entity<AttachmentFile>(entity =>
        {
            entity.Property(e => e.AttachmentFileName).HasMaxLength(50);
            entity.Property(e => e.CrDate).HasColumnType("datetime");

            entity.HasOne(d => d.Attachment).WithMany(p => p.AttachmentFiles)
                .HasForeignKey(d => d.AttachmentId)
                .HasConstraintName("FK_AttachmentFiles_Attachment");
        });

        modelBuilder.Entity<Carrier>(entity =>
        {
            entity.HasKey(e => e.CarrierId).HasName("PK__Carriers__CB8205791DCBA4F8");

            entity.Property(e => e.CarrierId).HasColumnName("CarrierID");
            entity.Property(e => e.CarrierName).HasMaxLength(100);
            entity.Property(e => e.ContactInfo).HasMaxLength(250);
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.Property(e => e.CountryName).HasMaxLength(50);
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.ToTable("Currency");

            entity.Property(e => e.CurrencyName).HasMaxLength(50);
        });

        modelBuilder.Entity<InstallmentsType>(entity =>
        {
            entity.ToTable("InstallmentsType");

            entity.Property(e => e.InstallmentsTypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.Property(e => e.ItemBarCode).HasMaxLength(50);
            entity.Property(e => e.ItemCode).HasMaxLength(50);
            entity.Property(e => e.ItemName).HasMaxLength(50);
        });

        modelBuilder.Entity<Offer>(entity =>
        {
            entity.ToTable("Offer");

            entity.Property(e => e.OfferId).HasColumnName("OfferID");
            entity.Property(e => e.OfferDate).HasColumnType("datetime");
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

            entity.HasOne(d => d.Attachment).WithMany(p => p.Offers)
                .HasForeignKey(d => d.AttachmentId)
                .HasConstraintName("FK_Offer_Attachment");

            entity.HasOne(d => d.OfferStatus).WithMany(p => p.Offers)
                .HasForeignKey(d => d.OfferStatusId)
                .HasConstraintName("FK_Offer_OfferStatus");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Offers)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("FK_Offer_Suppliers");
        });

        modelBuilder.Entity<OfferContract>(entity =>
        {
            entity.ToTable("OfferContract");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.OfferId).HasColumnName("OfferID");
            entity.Property(e => e.Percentage).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Attachment).WithMany(p => p.OfferContracts)
                .HasForeignKey(d => d.AttachmentId)
                .HasConstraintName("FK_OfferContract_Attachment");

            entity.HasOne(d => d.Currency).WithMany(p => p.OfferContracts)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OfferContract_Currency");

            entity.HasOne(d => d.InstallmentsType).WithMany(p => p.OfferContracts)
                .HasForeignKey(d => d.InstallmentsTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OfferContract_InstallmentsType");

            entity.HasOne(d => d.Offer).WithMany(p => p.OfferContracts)
                .HasForeignKey(d => d.OfferId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OfferContract_Offer");

            entity.HasOne(d => d.PaymentStates).WithMany(p => p.OfferContracts)
                .HasForeignKey(d => d.PaymentStatesId)
                .HasConstraintName("FK_OfferContract_PaymentStates");
        });

        modelBuilder.Entity<OfferItem>(entity =>
        {
            entity.Property(e => e.OfferItemId).ValueGeneratedNever();
            entity.Property(e => e.ItemQuantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ItemUnitPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OfferId).HasColumnName("OfferID");

            entity.HasOne(d => d.Currency).WithMany(p => p.OfferItems)
                .HasForeignKey(d => d.CurrencyId)
                .HasConstraintName("FK_OfferItems_Currency");

            entity.HasOne(d => d.Item).WithMany(p => p.OfferItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OfferItems_Items");

            entity.HasOne(d => d.Offer).WithMany(p => p.OfferItems)
                .HasForeignKey(d => d.OfferId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OfferItems_Offer");

            entity.HasOne(d => d.PrmotionFileAttachment).WithMany(p => p.OfferItems)
                .HasForeignKey(d => d.PrmotionFileAttachmentId)
                .HasConstraintName("FK_OfferItems_Attachment");
        });

        modelBuilder.Entity<OfferShippingContract>(entity =>
        {
            entity.ToTable("OfferShippingContract");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.Percentage).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Attachment).WithMany(p => p.OfferShippingContracts)
                .HasForeignKey(d => d.AttachmentId)
                .HasConstraintName("FK_OfferShippingContract_Attachment");

            entity.HasOne(d => d.Currency).WithMany(p => p.OfferShippingContracts)
                .HasForeignKey(d => d.CurrencyId)
                .HasConstraintName("FK_OfferShippingContract_Currency");

            entity.HasOne(d => d.InstallmentsType).WithMany(p => p.OfferShippingContracts)
                .HasForeignKey(d => d.InstallmentsTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OfferShippingContract_InstallmentsType");

            entity.HasOne(d => d.OfferShippingCost).WithMany(p => p.OfferShippingContracts)
                .HasForeignKey(d => d.OfferShippingCostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OfferShippingContract_OfferShippingCost");

            entity.HasOne(d => d.PaymentStates).WithMany(p => p.OfferShippingContracts)
                .HasForeignKey(d => d.PaymentStatesId)
                .HasConstraintName("FK_OfferShippingContract_PaymentStates");
        });

        modelBuilder.Entity<OfferShippingCost>(entity =>
        {
            entity.ToTable("OfferShippingCost");

            entity.Property(e => e.CarrierId).HasColumnName("CarrierID");
            entity.Property(e => e.OfferId).HasColumnName("OfferID");
            entity.Property(e => e.ShippingCost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ShippingCostNote).HasMaxLength(50);

            entity.HasOne(d => d.Attachment).WithMany(p => p.OfferShippingCosts)
                .HasForeignKey(d => d.AttachmentId)
                .HasConstraintName("FK_OfferShippingCost_Attachment");

            entity.HasOne(d => d.Carrier).WithMany(p => p.OfferShippingCosts)
                .HasForeignKey(d => d.CarrierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OfferShippingCost_Carriers");

            entity.HasOne(d => d.Offer).WithMany(p => p.OfferShippingCosts)
                .HasForeignKey(d => d.OfferId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OfferShippingCost_Offer");

            entity.HasOne(d => d.ShipmentType).WithMany(p => p.OfferShippingCosts)
                .HasForeignKey(d => d.ShipmentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OfferShippingCost_ShipmentType");
        });

        modelBuilder.Entity<OfferStatus>(entity =>
        {
            entity.ToTable("OfferStatus");

            entity.Property(e => e.OfferStatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.ToTable("PaymentMethod");

            entity.Property(e => e.PaymentMethodName).HasMaxLength(50);
        });

        modelBuilder.Entity<PaymentState>(entity =>
        {
            entity.HasKey(e => e.PaymentStatesId);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.RoleName).HasMaxLength(50);

            entity.HasOne(d => d.RoleType).WithMany(p => p.Roles)
                .HasForeignKey(d => d.RoleTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Roles_RoleType");
        });

        modelBuilder.Entity<RoleType>(entity =>
        {
            entity.ToTable("RoleType");

            entity.Property(e => e.RoleTypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<ShipmentType>(entity =>
        {
            entity.ToTable("ShipmentType");

            entity.Property(e => e.ShipmentTypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.Property(e => e.SupplierId).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.SupplierName).HasMaxLength(50);
            entity.Property(e => e.Websit).HasMaxLength(50);

            entity.HasOne(d => d.Attachment).WithMany(p => p.Suppliers)
                .HasForeignKey(d => d.AttachmentId)
                .HasConstraintName("FK_Suppliers_Attachment");

            entity.HasOne(d => d.Country).WithMany(p => p.Suppliers)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK_Suppliers_Countries");
        });

        modelBuilder.Entity<SupplierContact>(entity =>
        {
            entity.HasKey(e => e.SuppliersContactId);

            entity.ToTable("SupplierContact");

            entity.Property(e => e.PersonEmail).HasMaxLength(50);
            entity.Property(e => e.PersonName).HasMaxLength(50);
            entity.Property(e => e.PersonPhone).HasMaxLength(50);
            entity.Property(e => e.PersonPhoneTwo).HasMaxLength(50);
            entity.Property(e => e.PersonPosition).HasMaxLength(50);
            entity.Property(e => e.PersonWhatsapp).HasMaxLength(50);

            entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierContacts)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("FK_SupplierContact_Suppliers");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.PassWord).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRoles_Roles");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRoles_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
