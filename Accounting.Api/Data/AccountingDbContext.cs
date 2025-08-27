using Microsoft.EntityFrameworkCore;
using Accounting.Api.Models;

namespace Accounting.Api.Data
{
    public class AccountingDbContext : DbContext
    {
        public AccountingDbContext(DbContextOptions<AccountingDbContext> options) : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<ChartOfAccount> ChartOfAccounts { get; set; }
        public DbSet<JournalEntry> JournalEntries { get; set; }
        public DbSet<JournalEntryDetail> JournalEntryDetails { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

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
            // ChartOfAccount self-referencing relationship
            modelBuilder.Entity<ChartOfAccount>()
                .HasOne(coa => coa.ParentAccount)
                .WithMany(coa => coa.SubAccounts)
                .HasForeignKey(coa => coa.ParentAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            // JournalEntryDetail relationships
            modelBuilder.Entity<JournalEntryDetail>()
                .HasOne(jed => jed.JournalEntry)
                .WithMany(je => je.JournalEntryDetails)
                .HasForeignKey(jed => jed.EntryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JournalEntryDetail>()
                .HasOne(jed => jed.ChartOfAccount)
                .WithMany(coa => coa.JournalEntryDetails)
                .HasForeignKey(jed => jed.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            // Supplier relationship with ChartOfAccount
            modelBuilder.Entity<Supplier>()
                .HasOne(s => s.ChartOfAccount)
                .WithMany(coa => coa.Suppliers)
                .HasForeignKey(s => s.AccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureIndexes(ModelBuilder modelBuilder)
        {
            // Indexes for better performance
            modelBuilder.Entity<ChartOfAccount>()
                .HasIndex(coa => coa.AccountCode)
                .IsUnique();

            modelBuilder.Entity<ChartOfAccount>()
                .HasIndex(coa => coa.AccountType);

            modelBuilder.Entity<JournalEntry>()
                .HasIndex(je => je.EntryDate);

            modelBuilder.Entity<JournalEntry>()
                .HasIndex(je => je.OrderId);

            modelBuilder.Entity<JournalEntry>()
                .HasIndex(je => je.IsPosted);

            modelBuilder.Entity<JournalEntryDetail>()
                .HasIndex(jed => jed.AccountId);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Chart of Accounts
            modelBuilder.Entity<ChartOfAccount>().HasData(
                // الأصول (Assets)
                new ChartOfAccount { AccountId = 1, AccountCode = "101", AccountName = "البنك", AccountType = "Asset", ParentAccountId = null, IsActive = true, CreatedDate = DateTime.UtcNow },
                new ChartOfAccount { AccountId = 2, AccountCode = "102", AccountName = "النقدية", AccountType = "Asset", ParentAccountId = null, IsActive = true, CreatedDate = DateTime.UtcNow },
                new ChartOfAccount { AccountId = 3, AccountCode = "103", AccountName = "العملاء", AccountType = "Asset", ParentAccountId = null, IsActive = true, CreatedDate = DateTime.UtcNow },
                new ChartOfAccount { AccountId = 4, AccountCode = "104", AccountName = "المخزون", AccountType = "Asset", ParentAccountId = null, IsActive = true, CreatedDate = DateTime.UtcNow },
                new ChartOfAccount { AccountId = 5, AccountCode = "105", AccountName = "الأصول الثابتة", AccountType = "Asset", ParentAccountId = null, IsActive = true, CreatedDate = DateTime.UtcNow },

                // الخصوم (Liabilities)
                new ChartOfAccount { AccountId = 6, AccountCode = "201", AccountName = "الموردين", AccountType = "Liability", ParentAccountId = null, IsActive = true, CreatedDate = DateTime.UtcNow },
                new ChartOfAccount { AccountId = 7, AccountCode = "202", AccountName = "الضرائب المستحقة", AccountType = "Liability", ParentAccountId = null, IsActive = true, CreatedDate = DateTime.UtcNow },
                new ChartOfAccount { AccountId = 8, AccountCode = "203", AccountName = "الرواتب المستحقة", AccountType = "Liability", ParentAccountId = null, IsActive = true, CreatedDate = DateTime.UtcNow },

                // حقوق الملكية (Equity)
                new ChartOfAccount { AccountId = 9, AccountCode = "301", AccountName = "رأس المال", AccountType = "Equity", ParentAccountId = null, IsActive = true, CreatedDate = DateTime.UtcNow },
                new ChartOfAccount { AccountId = 10, AccountCode = "302", AccountName = "الأرباح المحتجزة", AccountType = "Equity", ParentAccountId = null, IsActive = true, CreatedDate = DateTime.UtcNow },
                new ChartOfAccount { AccountId = 11, AccountCode = "303", AccountName = "صافي الدخل", AccountType = "Equity", ParentAccountId = null, IsActive = true, CreatedDate = DateTime.UtcNow },

                // الإيرادات (Revenue)
                new ChartOfAccount { AccountId = 12, AccountCode = "401", AccountName = "المبيعات", AccountType = "Revenue", ParentAccountId = null, IsActive = true, CreatedDate = DateTime.UtcNow },
                new ChartOfAccount { AccountId = 13, AccountCode = "402", AccountName = "إيرادات الخدمات", AccountType = "Revenue", ParentAccountId = null, IsActive = true, CreatedDate = DateTime.UtcNow },
                new ChartOfAccount { AccountId = 14, AccountCode = "403", AccountName = "إيرادات أخرى", AccountType = "Revenue", ParentAccountId = null, IsActive = true, CreatedDate = DateTime.UtcNow },

                // المصروفات (Expenses)
                new ChartOfAccount { AccountId = 15, AccountCode = "501", AccountName = "المشتريات", AccountType = "Expense", ParentAccountId = null, IsActive = true, CreatedDate = DateTime.UtcNow },
                new ChartOfAccount { AccountId = 16, AccountCode = "502", AccountName = "تكاليف الشحن", AccountType = "Expense", ParentAccountId = null, IsActive = true, CreatedDate = DateTime.UtcNow },
                new ChartOfAccount { AccountId = 17, AccountCode = "503", AccountName = "المصروفات الإدارية", AccountType = "Expense", ParentAccountId = null, IsActive = true, CreatedDate = DateTime.UtcNow },
                new ChartOfAccount { AccountId = 18, AccountCode = "504", AccountName = "المصروفات التشغيلية", AccountType = "Expense", ParentAccountId = null, IsActive = true, CreatedDate = DateTime.UtcNow },
                new ChartOfAccount { AccountId = 19, AccountCode = "505", AccountName = "مصروفات التسويق", AccountType = "Expense", ParentAccountId = null, IsActive = true, CreatedDate = DateTime.UtcNow },
                new ChartOfAccount { AccountId = 20, AccountCode = "506", AccountName = "مصروفات أخرى", AccountType = "Expense", ParentAccountId = null, IsActive = true, CreatedDate = DateTime.UtcNow }
            );
        }
    }
}
