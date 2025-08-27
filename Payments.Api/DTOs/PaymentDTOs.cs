using System.ComponentModel.DataAnnotations;
using Payments.Api.Models;

namespace Payments.Api.DTOs
{
    // DTOs لدفعيات العملاء
    public class CreateCustomerPaymentDto
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
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int PaymentStatesId { get; set; }

        [Required]
        public int CurrencyId { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        [Required]
        public int CreatedBy { get; set; }
    }

    public class UpdateCustomerPaymentDto
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "المبلغ يجب أن يكون أكبر من صفر")]
        public decimal? Amount { get; set; }

        public int? PaymentMethodId { get; set; }

        public DateTime? PaymentDate { get; set; }

        public int? PaymentStatesId { get; set; }

        public int? CurrencyId { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }
    }

    // DTOs لدفعيات الموردين
    public class CreateSupplierPaymentDto
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
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int PaymentStatesId { get; set; }

        [Required]
        public int CurrencyId { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        [Required]
        public int CreatedBy { get; set; }
    }

    public class UpdateSupplierPaymentDto
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "المبلغ يجب أن يكون أكبر من صفر")]
        public decimal? Amount { get; set; }

        public int? PaymentMethodId { get; set; }

        public DateTime? PaymentDate { get; set; }

        public int? PaymentStatesId { get; set; }

        public int? CurrencyId { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }
    }

    // DTOs لطرق الدفع
    public class CreatePaymentMethodDto
    {
        [Required]
        [MaxLength(50)]
        public string PaymentMethodName { get; set; } = string.Empty;
    }

    public class UpdatePaymentMethodDto
    {
        [Required]
        [MaxLength(50)]
        public string PaymentMethodName { get; set; } = string.Empty;
    }

    // DTOs لحالات الدفع
    public class CreatePaymentStateDto
    {
        [Required]
        [MaxLength(200)]
        public string PaymentStatesName { get; set; } = string.Empty;
    }

    public class UpdatePaymentStateDto
    {
        [Required]
        [MaxLength(200)]
        public string PaymentStatesName { get; set; } = string.Empty;
    }

    // DTOs للعملات
    public class CreateCurrencyDto
    {
        [Required]
        [MaxLength(50)]
        public string CurrencyName { get; set; } = string.Empty;

        [MaxLength(10)]
        public string? CurrencyCode { get; set; }

        [MaxLength(10)]
        public string? CurrencySymbol { get; set; }

        [Range(0.0001, double.MaxValue, ErrorMessage = "سعر الصرف يجب أن يكون أكبر من صفر")]
        public decimal ExchangeRate { get; set; } = 1.0000m;
    }

    public class UpdateCurrencyDto
    {
        [Required]
        [MaxLength(50)]
        public string CurrencyName { get; set; } = string.Empty;

        [MaxLength(10)]
        public string? CurrencyCode { get; set; }

        [MaxLength(10)]
        public string? CurrencySymbol { get; set; }

        [Range(0.0001, double.MaxValue, ErrorMessage = "سعر الصرف يجب أن يكون أكبر من صفر")]
        public decimal ExchangeRate { get; set; }

        public bool IsActive { get; set; } = true;
    }

    // DTOs لأنواع التقسيط
    public class CreateInstallmentsTypeDto
    {
        [Required]
        [MaxLength(50)]
        public string InstallmentsTypeName { get; set; } = string.Empty;

        [Required]
        [Range(1, 60, ErrorMessage = "عدد الدفعات يجب أن يكون بين 1 و 60")]
        public int NumberOfInstallments { get; set; } = 1;
    }

    public class UpdateInstallmentsTypeDto
    {
        [Required]
        [MaxLength(50)]
        public string InstallmentsTypeName { get; set; } = string.Empty;

        [Required]
        [Range(1, 60, ErrorMessage = "عدد الدفعات يجب أن يكون بين 1 و 60")]
        public int NumberOfInstallments { get; set; }

        public bool IsActive { get; set; } = true;
    }

    // Response DTOs
    public class CustomerPaymentResponseDto
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethodName { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
        public string PaymentStateName { get; set; } = string.Empty;
        public string CurrencyName { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class SupplierPaymentResponseDto
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string PaymentMethodName { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
        public string PaymentStateName { get; set; } = string.Empty;
        public string CurrencyName { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    // DTOs للتقارير
    public class PaymentReportRequestDto
    {
        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public int? PaymentMethodId { get; set; }
        public int? CurrencyId { get; set; }
    }

    public class PaymentStatisticsDto
    {
        public decimal TotalCustomerPayments { get; set; }
        public decimal TotalSupplierPayments { get; set; }
        public decimal NetAmount { get; set; }
        public int TotalCustomerPaymentCount { get; set; }
        public int TotalSupplierPaymentCount { get; set; }
        public decimal AverageCustomerPayment { get; set; }
        public decimal AverageSupplierPayment { get; set; }
        public List<PaymentMethodStatistics> PaymentMethodStats { get; set; } = new List<PaymentMethodStatistics>();
        public List<CurrencyStatistics> CurrencyStats { get; set; } = new List<CurrencyStatistics>();
    }

    public class PaymentMethodStatistics
    {
        public int PaymentMethodId { get; set; }
        public string PaymentMethodName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public int PaymentCount { get; set; }
        public decimal Percentage { get; set; }
    }

    public class CurrencyStatistics
    {
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public int PaymentCount { get; set; }
        public decimal Percentage { get; set; }
    }
}
