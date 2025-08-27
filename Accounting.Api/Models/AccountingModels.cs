using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounting.Api.Models
{
    // دليل الحسابات
    [Table("ChartOfAccounts")]
    public class ChartOfAccount
    {
        [Key]
        [Column("AccountId")]
        public int AccountId { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("AccountCode")]
        public string AccountCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("AccountName")]
        public string AccountName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("AccountType")]
        public string AccountType { get; set; } = string.Empty; // Asset, Liability, Equity, Revenue, Expense

        [Column("ParentAccountId")]
        public int? ParentAccountId { get; set; }

        [Column("IsActive")]
        public bool IsActive { get; set; } = true;

        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("UpdatedDate")]
        public DateTime? UpdatedDate { get; set; }

        // Navigation properties
        [ForeignKey("ParentAccountId")]
        public virtual ChartOfAccount? ParentAccount { get; set; }

        public virtual ICollection<ChartOfAccount> SubAccounts { get; set; } = new List<ChartOfAccount>();
        public virtual ICollection<JournalEntryDetail> JournalEntryDetails { get; set; } = new List<JournalEntryDetail>();
        public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
    }

    // قيود اليومية
    [Table("JournalEntries")]
    public class JournalEntry
    {
        [Key]
        [Column("EntryId")]
        public int EntryId { get; set; }

        [Column("OrderId")]
        public int? OrderId { get; set; }

        [Required]
        [Column("EntryDate")]
        public DateTime EntryDate { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(500)]
        [Column("Description")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Column("TotalAmount", TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [Column("CreatedBy")]
        public int CreatedBy { get; set; }

        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("IsPosted")]
        public bool IsPosted { get; set; } = false;

        [Column("PostedDate")]
        public DateTime? PostedDate { get; set; }

        [Column("PostedBy")]
        public int? PostedBy { get; set; }

        // Navigation properties
        public virtual ICollection<JournalEntryDetail> JournalEntryDetails { get; set; } = new List<JournalEntryDetail>();
    }

    // تفاصيل قيود اليومية
    [Table("JournalEntryDetails")]
    public class JournalEntryDetail
    {
        [Key]
        [Column("DetailId")]
        public int DetailId { get; set; }

        [Required]
        [Column("EntryId")]
        public int EntryId { get; set; }

        [Required]
        [Column("AccountId")]
        public int AccountId { get; set; }

        [Required]
        [Column("Debit", TypeName = "decimal(18,2)")]
        public decimal Debit { get; set; } = 0;

        [Required]
        [Column("Credit", TypeName = "decimal(18,2)")]
        public decimal Credit { get; set; } = 0;

        [MaxLength(500)]
        [Column("Description")]
        public string? Description { get; set; }

        // Navigation properties
        [ForeignKey("EntryId")]
        public virtual JournalEntry JournalEntry { get; set; } = null!;

        [ForeignKey("AccountId")]
        public virtual ChartOfAccount ChartOfAccount { get; set; } = null!;
    }

    // الموردين (مرجع من Suppliers.Api)
    [Table("Suppliers")]
    public class Supplier
    {
        [Key]
        [Column("SupplierId")]
        public int SupplierId { get; set; }

        [MaxLength(50)]
        [Column("SupplierName")]
        public string? SupplierName { get; set; }

        [MaxLength(50)]
        [Column("Phone")]
        public string? Phone { get; set; }

        [MaxLength(50)]
        [Column("Email")]
        public string? Email { get; set; }

        [MaxLength(50)]
        [Column("Address")]
        public string? Address { get; set; }

        [MaxLength(50)]
        [Column("Websit")]
        public string? Websit { get; set; }

        [Column("CountryId")]
        public int? CountryId { get; set; }

        [Column("IsActive")]
        public bool? IsActive { get; set; }

        [MaxLength(500)]
        [Column("SupplierNote")]
        public string? SupplierNote { get; set; }

        [Column("AttachmentId")]
        public int? AttachmentId { get; set; }

        [Column("AccountId")]
        public int? AccountId { get; set; }

        // Navigation properties
        [ForeignKey("AccountId")]
        public virtual ChartOfAccount? ChartOfAccount { get; set; }
    }

    // أنواع الحسابات
    public enum AccountType
    {
        Asset = 1,      // الأصول
        Liability = 2,  // الخصوم
        Equity = 3,     // حقوق الملكية
        Revenue = 4,    // الإيرادات
        Expense = 5     // المصروفات
    }

    // أنواع القيود
    public enum JournalEntryType
    {
        Manual = 1,         // يدوي
        CustomerPayment = 2, // دفع عميل
        SupplierPayment = 3, // دفع مورد
        Order = 4,          // طلب
        Offer = 5,          // عرض
        Adjustment = 6      // تسوية
    }

    // تقارير الحسابات
    public class AccountBalance
    {
        public int AccountId { get; set; }
        public string AccountCode { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public decimal DebitBalance { get; set; }
        public decimal CreditBalance { get; set; }
        public decimal NetBalance { get; set; }
        public bool IsDebit { get; set; }
    }

    public class TrialBalance
    {
        public DateTime AsOfDate { get; set; }
        public List<AccountBalance> AccountBalances { get; set; } = new List<AccountBalance>();
        public decimal TotalDebits { get; set; }
        public decimal TotalCredits { get; set; }
        public bool IsBalanced { get; set; }
    }

    public class IncomeStatement
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal NetIncome { get; set; }
        public List<AccountBalance> RevenueAccounts { get; set; } = new List<AccountBalance>();
        public List<AccountBalance> ExpenseAccounts { get; set; } = new List<AccountBalance>();
    }

    public class BalanceSheet
    {
        public DateTime AsOfDate { get; set; }
        public decimal TotalAssets { get; set; }
        public decimal TotalLiabilities { get; set; }
        public decimal TotalEquity { get; set; }
        public List<AccountBalance> AssetAccounts { get; set; } = new List<AccountBalance>();
        public List<AccountBalance> LiabilityAccounts { get; set; } = new List<AccountBalance>();
        public List<AccountBalance> EquityAccounts { get; set; } = new List<AccountBalance>();
        public bool IsBalanced { get; set; }
    }

    // إحصائيات محاسبية
    public class AccountingStatistics
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int TotalJournalEntries { get; set; }
        public int PostedJournalEntries { get; set; }
        public int PendingJournalEntries { get; set; }
        public decimal TotalDebits { get; set; }
        public decimal TotalCredits { get; set; }
        public decimal NetAmount { get; set; }
        public List<AccountTypeStatistics> AccountTypeStats { get; set; } = new List<AccountTypeStatistics>();
    }

    public class AccountTypeStatistics
    {
        public string AccountType { get; set; } = string.Empty;
        public int AccountCount { get; set; }
        public decimal TotalDebitBalance { get; set; }
        public decimal TotalCreditBalance { get; set; }
        public decimal NetBalance { get; set; }
    }
}
