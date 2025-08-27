using System.ComponentModel.DataAnnotations;
using Accounting.Api.Models;

namespace Accounting.Api.DTOs
{
    // DTOs لدليل الحسابات
    public class CreateChartOfAccountDto
    {
        [Required]
        [MaxLength(20)]
        public string AccountCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string AccountName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string AccountType { get; set; } = string.Empty;

        public int? ParentAccountId { get; set; }
    }

    public class UpdateChartOfAccountDto
    {
        [Required]
        [MaxLength(20)]
        public string AccountCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string AccountName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string AccountType { get; set; } = string.Empty;

        public int? ParentAccountId { get; set; }

        public bool IsActive { get; set; } = true;
    }

    // DTOs لقيود اليومية
    public class CreateJournalEntryDto
    {
        public int? OrderId { get; set; }

        [Required]
        public DateTime EntryDate { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int CreatedBy { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "يجب أن تحتوي القيد على مدين ودائن على الأقل")]
        public List<CreateJournalEntryDetailDto> Details { get; set; } = new List<CreateJournalEntryDetailDto>();
    }

    public class CreateJournalEntryDetailDto
    {
        [Required]
        public int AccountId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "المبلغ المدين يجب أن يكون أكبر من أو يساوي صفر")]
        public decimal Debit { get; set; } = 0;

        [Range(0, double.MaxValue, ErrorMessage = "المبلغ الدائن يجب أن يكون أكبر من أو يساوي صفر")]
        public decimal Credit { get; set; } = 0;

        [MaxLength(500)]
        public string? Description { get; set; }
    }

    public class UpdateJournalEntryDto
    {
        public int? OrderId { get; set; }

        public DateTime? EntryDate { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public List<UpdateJournalEntryDetailDto>? Details { get; set; }
    }

    public class UpdateJournalEntryDetailDto
    {
        public int DetailId { get; set; }

        public int? AccountId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "المبلغ المدين يجب أن يكون أكبر من أو يساوي صفر")]
        public decimal? Debit { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "المبلغ الدائن يجب أن يكون أكبر من أو يساوي صفر")]
        public decimal? Credit { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }
    }

    // DTOs للتقارير
    public class TrialBalanceRequestDto
    {
        [Required]
        public DateTime AsOfDate { get; set; }

        public int? AccountId { get; set; }
        public string? AccountType { get; set; }
    }

    public class IncomeStatementRequestDto
    {
        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }
    }

    public class BalanceSheetRequestDto
    {
        [Required]
        public DateTime AsOfDate { get; set; }
    }

    public class AccountBalanceRequestDto
    {
        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        public int? AccountId { get; set; }
        public string? AccountType { get; set; }
    }

    // Response DTOs
    public class ChartOfAccountResponseDto
    {
        public int AccountId { get; set; }
        public string AccountCode { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public int? ParentAccountId { get; set; }
        public string? ParentAccountName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public List<ChartOfAccountResponseDto> SubAccounts { get; set; } = new List<ChartOfAccountResponseDto>();
    }

    public class JournalEntryResponseDto
    {
        public int EntryId { get; set; }
        public int? OrderId { get; set; }
        public DateTime EntryDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsPosted { get; set; }
        public DateTime? PostedDate { get; set; }
        public int? PostedBy { get; set; }
        public List<JournalEntryDetailResponseDto> Details { get; set; } = new List<JournalEntryDetailResponseDto>();
    }

    public class JournalEntryDetailResponseDto
    {
        public int DetailId { get; set; }
        public int EntryId { get; set; }
        public int AccountId { get; set; }
        public string AccountCode { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string? Description { get; set; }
    }

    public class AccountBalanceResponseDto
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

    public class TrialBalanceResponseDto
    {
        public DateTime AsOfDate { get; set; }
        public List<AccountBalanceResponseDto> AccountBalances { get; set; } = new List<AccountBalanceResponseDto>();
        public decimal TotalDebits { get; set; }
        public decimal TotalCredits { get; set; }
        public bool IsBalanced { get; set; }
    }

    public class IncomeStatementResponseDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal NetIncome { get; set; }
        public List<AccountBalanceResponseDto> RevenueAccounts { get; set; } = new List<AccountBalanceResponseDto>();
        public List<AccountBalanceResponseDto> ExpenseAccounts { get; set; } = new List<AccountBalanceResponseDto>();
    }

    public class BalanceSheetResponseDto
    {
        public DateTime AsOfDate { get; set; }
        public decimal TotalAssets { get; set; }
        public decimal TotalLiabilities { get; set; }
        public decimal TotalEquity { get; set; }
        public List<AccountBalanceResponseDto> AssetAccounts { get; set; } = new List<AccountBalanceResponseDto>();
        public List<AccountBalanceResponseDto> LiabilityAccounts { get; set; } = new List<AccountBalanceResponseDto>();
        public List<AccountBalanceResponseDto> EquityAccounts { get; set; } = new List<AccountBalanceResponseDto>();
        public bool IsBalanced { get; set; }
    }

    // DTOs للإحصائيات
    public class AccountingStatisticsResponseDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int TotalJournalEntries { get; set; }
        public int PostedJournalEntries { get; set; }
        public int PendingJournalEntries { get; set; }
        public decimal TotalDebits { get; set; }
        public decimal TotalCredits { get; set; }
        public decimal NetAmount { get; set; }
        public List<AccountTypeStatisticsResponseDto> AccountTypeStats { get; set; } = new List<AccountTypeStatisticsResponseDto>();
    }

    public class AccountTypeStatisticsResponseDto
    {
        public string AccountType { get; set; } = string.Empty;
        public int AccountCount { get; set; }
        public decimal TotalDebitBalance { get; set; }
        public decimal TotalCreditBalance { get; set; }
        public decimal NetBalance { get; set; }
    }

    // DTOs للقيود التلقائية
    public class CreateCustomerPaymentJournalEntryDto
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "المبلغ يجب أن يكون أكبر من صفر")]
        public decimal Amount { get; set; }

        [Required]
        public int PaymentMethodId { get; set; }

        [Required]
        public int CurrencyId { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }
    }

    public class CreateSupplierPaymentJournalEntryDto
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "المبلغ يجب أن يكون أكبر من صفر")]
        public decimal Amount { get; set; }

        [Required]
        public int PaymentMethodId { get; set; }

        [Required]
        public int CurrencyId { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
