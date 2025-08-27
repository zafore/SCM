using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payments.Api.Models
{
    // دفعيات العملاء
    [Table("CustomerPayments")]
    public class CustomerPayment
    {
        [Key]
        [Column("PaymentId")]
        public int PaymentId { get; set; }

        [Required]
        [Column("OrderId")]
        public int OrderId { get; set; }

        [Required]
        [Column("CustomerId")]
        public int CustomerId { get; set; }

        [Required]
        [Column("Amount", TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [Column("PaymentMethodId")]
        public int PaymentMethodId { get; set; }

        [Required]
        [Column("PaymentDate")]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("PaymentStatesId")]
        public int PaymentStatesId { get; set; }

        [Required]
        [Column("CurrencyId")]
        public int CurrencyId { get; set; }

        [MaxLength(500)]
        [Column("Notes")]
        public string? Notes { get; set; }

        [Required]
        [Column("CreatedBy")]
        public int CreatedBy { get; set; }

        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("PaymentMethodId")]
        public virtual PaymentMethod PaymentMethod { get; set; } = null!;

        [ForeignKey("PaymentStatesId")]
        public virtual PaymentState PaymentState { get; set; } = null!;

        [ForeignKey("CurrencyId")]
        public virtual Currency Currency { get; set; } = null!;
    }

    // دفعيات الموردين
    [Table("SupplierPayments")]
    public class SupplierPayment
    {
        [Key]
        [Column("PaymentId")]
        public int PaymentId { get; set; }

        [Required]
        [Column("OrderId")]
        public int OrderId { get; set; }

        [Required]
        [Column("SupplierId")]
        public int SupplierId { get; set; }

        [Required]
        [Column("Amount", TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [Column("PaymentMethodId")]
        public int PaymentMethodId { get; set; }

        [Required]
        [Column("PaymentDate")]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("PaymentStatesId")]
        public int PaymentStatesId { get; set; }

        [Required]
        [Column("CurrencyId")]
        public int CurrencyId { get; set; }

        [MaxLength(500)]
        [Column("Notes")]
        public string? Notes { get; set; }

        [Required]
        [Column("CreatedBy")]
        public int CreatedBy { get; set; }

        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("PaymentMethodId")]
        public virtual PaymentMethod PaymentMethod { get; set; } = null!;

        [ForeignKey("PaymentStatesId")]
        public virtual PaymentState PaymentState { get; set; } = null!;

        [ForeignKey("CurrencyId")]
        public virtual Currency Currency { get; set; } = null!;

        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; } = null!;
    }

    // طرق الدفع
    [Table("PaymentMethod")]
    public class PaymentMethod
    {
        [Key]
        [Column("PaymentMethodId")]
        public int PaymentMethodId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("PaymentMethodName")]
        public string PaymentMethodName { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<CustomerPayment> CustomerPayments { get; set; } = new List<CustomerPayment>();
        public virtual ICollection<SupplierPayment> SupplierPayments { get; set; } = new List<SupplierPayment>();
    }

    // حالات الدفع
    [Table("PaymentStates")]
    public class PaymentState
    {
        [Key]
        [Column("PaymentStatesId")]
        public int PaymentStatesId { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("PaymentStatesName")]
        public string PaymentStatesName { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<CustomerPayment> CustomerPayments { get; set; } = new List<CustomerPayment>();
        public virtual ICollection<SupplierPayment> SupplierPayments { get; set; } = new List<SupplierPayment>();
    }

    // العملات
    [Table("Currency")]
    public class Currency
    {
        [Key]
        [Column("CurrencyId")]
        public int CurrencyId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("CurrencyName")]
        public string CurrencyName { get; set; } = string.Empty;

        [MaxLength(10)]
        [Column("CurrencyCode")]
        public string? CurrencyCode { get; set; }

        [MaxLength(10)]
        [Column("CurrencySymbol")]
        public string? CurrencySymbol { get; set; }

        [Column("ExchangeRate", TypeName = "decimal(18,4)")]
        public decimal ExchangeRate { get; set; } = 1.0000m;

        [Column("IsActive")]
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<CustomerPayment> CustomerPayments { get; set; } = new List<CustomerPayment>();
        public virtual ICollection<SupplierPayment> SupplierPayments { get; set; } = new List<SupplierPayment>();
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
        public virtual ICollection<SupplierPayment> SupplierPayments { get; set; } = new List<SupplierPayment>();
    }

    // أنواع التقسيط
    [Table("InstallmentsType")]
    public class InstallmentsType
    {
        [Key]
        [Column("InstallmentsTypeId")]
        public int InstallmentsTypeId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("InstallmentsTypeName")]
        public string InstallmentsTypeName { get; set; } = string.Empty;

        [Column("NumberOfInstallments")]
        public int NumberOfInstallments { get; set; } = 1;

        [Column("IsActive")]
        public bool IsActive { get; set; } = true;
    }

    // تقارير المدفوعات
    public class PaymentReport
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal TotalCustomerPayments { get; set; }
        public decimal TotalSupplierPayments { get; set; }
        public decimal NetAmount { get; set; }
        public int TotalCustomerPaymentCount { get; set; }
        public int TotalSupplierPaymentCount { get; set; }
        public List<PaymentSummary> PaymentSummaries { get; set; } = new List<PaymentSummary>();
    }

    public class PaymentSummary
    {
        public DateTime PaymentDate { get; set; }
        public decimal CustomerPayments { get; set; }
        public decimal SupplierPayments { get; set; }
        public decimal NetAmount { get; set; }
        public int CustomerPaymentCount { get; set; }
        public int SupplierPaymentCount { get; set; }
    }
}
