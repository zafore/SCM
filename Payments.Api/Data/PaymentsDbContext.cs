using Microsoft.EntityFrameworkCore;
using Payments.Api.Models;

namespace Payments.Api.Data
{
    public class PaymentsDbContext : DbContext
    {
        public PaymentsDbContext(DbContextOptions<PaymentsDbContext> options) : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<CustomerPayment> CustomerPayments { get; set; }
        public DbSet<SupplierPayment> SupplierPayments { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<PaymentState> PaymentStates { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<InstallmentsType> InstallmentsTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and constraints
            ConfigureRelationships(modelBuilder);
            ConfigureIndexes(modelBuilder);
            SeedData(modelBuilder);
        }

        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            // CustomerPayment relationships
            modelBuilder.Entity<CustomerPayment>()
                .HasOne(cp => cp.PaymentMethod)
                .WithMany(pm => pm.CustomerPayments)
                .HasForeignKey(cp => cp.PaymentMethodId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CustomerPayment>()
                .HasOne(cp => cp.PaymentState)
                .WithMany(ps => ps.CustomerPayments)
                .HasForeignKey(cp => cp.PaymentStatesId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CustomerPayment>()
                .HasOne(cp => cp.Currency)
                .WithMany(c => c.CustomerPayments)
                .HasForeignKey(cp => cp.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            // SupplierPayment relationships
            modelBuilder.Entity<SupplierPayment>()
                .HasOne(sp => sp.PaymentMethod)
                .WithMany(pm => pm.SupplierPayments)
                .HasForeignKey(sp => sp.PaymentMethodId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SupplierPayment>()
                .HasOne(sp => sp.PaymentState)
                .WithMany(ps => ps.SupplierPayments)
                .HasForeignKey(sp => sp.PaymentStatesId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SupplierPayment>()
                .HasOne(sp => sp.Currency)
                .WithMany(c => c.SupplierPayments)
                .HasForeignKey(sp => sp.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SupplierPayment>()
                .HasOne(sp => sp.Supplier)
                .WithMany(s => s.SupplierPayments)
                .HasForeignKey(sp => sp.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureIndexes(ModelBuilder modelBuilder)
        {
            // Indexes for better performance
            modelBuilder.Entity<CustomerPayment>()
                .HasIndex(cp => cp.PaymentDate);

            modelBuilder.Entity<CustomerPayment>()
                .HasIndex(cp => cp.OrderId);

            modelBuilder.Entity<CustomerPayment>()
                .HasIndex(cp => cp.CustomerId);

            modelBuilder.Entity<SupplierPayment>()
                .HasIndex(sp => sp.PaymentDate);

            modelBuilder.Entity<SupplierPayment>()
                .HasIndex(sp => sp.OrderId);

            modelBuilder.Entity<SupplierPayment>()
                .HasIndex(sp => sp.SupplierId);

            modelBuilder.Entity<PaymentMethod>()
                .HasIndex(pm => pm.PaymentMethodName);

            modelBuilder.Entity<Currency>()
                .HasIndex(c => c.CurrencyCode);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Payment Methods
            modelBuilder.Entity<PaymentMethod>().HasData(
                new PaymentMethod { PaymentMethodId = 1, PaymentMethodName = "نقدي" },
                new PaymentMethod { PaymentMethodId = 2, PaymentMethodName = "تحويل بنكي" },
                new PaymentMethod { PaymentMethodId = 3, PaymentMethodName = "شيك" },
                new PaymentMethod { PaymentMethodId = 4, PaymentMethodName = "بطاقة ائتمان" },
                new PaymentMethod { PaymentMethodId = 5, PaymentMethodName = "دفع إلكتروني" }
            );

            // Seed Payment States
            modelBuilder.Entity<PaymentState>().HasData(
                new PaymentState { PaymentStatesId = 1, PaymentStatesName = "معلق" },
                new PaymentState { PaymentStatesId = 2, PaymentStatesName = "مدفوع" },
                new PaymentState { PaymentStatesId = 3, PaymentStatesName = "مدفوع جزئياً" },
                new PaymentState { PaymentStatesId = 4, PaymentStatesName = "فشل" },
                new PaymentState { PaymentStatesId = 5, PaymentStatesName = "مسترد" }
            );

            // Seed Currencies
            modelBuilder.Entity<Currency>().HasData(
                new Currency { CurrencyId = 1, CurrencyName = "ريال سعودي", CurrencyCode = "SAR", CurrencySymbol = "ر.س", ExchangeRate = 1.0000m },
                new Currency { CurrencyId = 2, CurrencyName = "دولار أمريكي", CurrencyCode = "USD", CurrencySymbol = "$", ExchangeRate = 3.7500m },
                new Currency { CurrencyId = 3, CurrencyName = "يورو", CurrencyCode = "EUR", CurrencySymbol = "€", ExchangeRate = 4.1000m }
            );

            // Seed Installments Types
            modelBuilder.Entity<InstallmentsType>().HasData(
                new InstallmentsType { InstallmentsTypeId = 1, InstallmentsTypeName = "دفعة واحدة", NumberOfInstallments = 1 },
                new InstallmentsType { InstallmentsTypeId = 2, InstallmentsTypeName = "دفعتين", NumberOfInstallments = 2 },
                new InstallmentsType { InstallmentsTypeId = 3, InstallmentsTypeName = "ثلاث دفعات", NumberOfInstallments = 3 },
                new InstallmentsType { InstallmentsTypeId = 4, InstallmentsTypeName = "أربع دفعات", NumberOfInstallments = 4 },
                new InstallmentsType { InstallmentsTypeId = 5, InstallmentsTypeName = "ست دفعات", NumberOfInstallments = 6 },
                new InstallmentsType { InstallmentsTypeId = 6, InstallmentsTypeName = "اثنتا عشرة دفعة", NumberOfInstallments = 12 }
            );
        }
    }
}
