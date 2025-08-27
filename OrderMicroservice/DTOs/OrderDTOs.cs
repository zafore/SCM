using System.ComponentModel.DataAnnotations;
using OrderMicroservice.Models;

namespace OrderMicroservice.DTOs
{
    // Order DTOs
    public class CreateOrderDto
    {
        [Required]
        public OrderType OrderType { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public int? SupplierId { get; set; }

        public int? OfferId { get; set; }

        public DateTime? RequiredDate { get; set; }

        [MaxLength(200)]
        public string? ShippingAddress { get; set; }

        [MaxLength(200)]
        public string? BillingAddress { get; set; }

        [MaxLength(50)]
        public string? PaymentMethod { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        public int? CreatedBy { get; set; }

        [MaxLength(50)]
        public string? CreatedByName { get; set; }

        public List<CreateOrderItemDto>? OrderItems { get; set; }
    }

    public class CreateOrderFromOfferDto
    {
        [Required]
        public OrderType OrderType { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [Required]
        public int OfferId { get; set; }

        public DateTime? RequiredDate { get; set; }

        [MaxLength(200)]
        public string? ShippingAddress { get; set; }

        [MaxLength(200)]
        public string? BillingAddress { get; set; }

        [MaxLength(50)]
        public string? PaymentMethod { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        public int? CreatedBy { get; set; }

        [MaxLength(50)]
        public string? CreatedByName { get; set; }

        public List<CreateOrderItemDto>? OrderItems { get; set; }
    }

    public class UpdateOrderDto
    {
        public DateTime? RequiredDate { get; set; }

        [MaxLength(200)]
        public string? ShippingAddress { get; set; }

        [MaxLength(200)]
        public string? BillingAddress { get; set; }

        [MaxLength(50)]
        public string? PaymentMethod { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }
    }

    public class UpdateOrderStatusDto
    {
        [Required]
        public OrderStatus NewStatus { get; set; }

        [MaxLength(500)]
        public string? Comments { get; set; }

        public int? ChangedBy { get; set; }

        [MaxLength(50)]
        public string? ChangedByName { get; set; }
    }

    // Order Item DTOs
    public class CreateOrderItemDto
    {
        [Required]
        public int ItemId { get; set; }

        [Required]
        [MaxLength(100)]
        public string ItemName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? ItemCode { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public decimal Quantity { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Unit price must be non-negative")]
        public decimal UnitPrice { get; set; }

        [Range(0, 100, ErrorMessage = "Discount percentage must be between 0 and 100")]
        public decimal DiscountPercentage { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Discount amount must be non-negative")]
        public decimal DiscountAmount { get; set; }

        [Range(0, 100, ErrorMessage = "Tax rate must be between 0 and 100")]
        public decimal TaxRate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Tax amount must be non-negative")]
        public decimal TaxAmount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Line total must be non-negative")]
        public decimal LineTotal { get; set; }

        public int? WarehouseId { get; set; }

        [MaxLength(50)]
        public string? Location { get; set; }
    }

    public class UpdateOrderItemDto
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public decimal? Quantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Unit price must be non-negative")]
        public decimal? UnitPrice { get; set; }

        [Range(0, 100, ErrorMessage = "Discount percentage must be between 0 and 100")]
        public decimal? DiscountPercentage { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Discount amount must be non-negative")]
        public decimal? DiscountAmount { get; set; }

        [Range(0, 100, ErrorMessage = "Tax rate must be between 0 and 100")]
        public decimal? TaxRate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Tax amount must be non-negative")]
        public decimal? TaxAmount { get; set; }

        public int? WarehouseId { get; set; }

        [MaxLength(50)]
        public string? Location { get; set; }
    }

    // Response DTOs
    public class OrderSummaryDto
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public OrderType OrderType { get; set; }
        public int CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public int? OfferId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public int ItemCount { get; set; }
    }

    public class OrderStatusHistoryDto
    {
        public int StatusHistoryId { get; set; }
        public int OrderId { get; set; }
        public OrderStatus PreviousStatus { get; set; }
        public OrderStatus NewStatus { get; set; }
        public string? Comments { get; set; }
        public int? ChangedBy { get; set; }
        public string? ChangedByName { get; set; }
        public DateTime ChangedDate { get; set; }
    }

    public class OrderStatisticsDto
    {
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int ConfirmedOrders { get; set; }
        public int ProcessingOrders { get; set; }
        public int ShippedOrders { get; set; }
        public int DeliveredOrders { get; set; }
        public int CancelledOrders { get; set; }
        public decimal TotalOrderValue { get; set; }
        public decimal AverageOrderValue { get; set; }
        public int OrdersThisMonth { get; set; }
        public decimal MonthlyOrderValue { get; set; }
    }

    // Offer Integration DTOs
    public class OfferToOrderDto
    {
        public int OfferId { get; set; }
        public string OfferName { get; set; } = string.Empty;
        public string OfferDescription { get; set; } = string.Empty;
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public DateTime OfferDate { get; set; }
        public List<OfferItemToOrderDto> OfferItems { get; set; } = new List<OfferItemToOrderDto>();
    }

    public class OfferItemToOrderDto
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string? ItemCode { get; set; }
        public decimal ItemQuantity { get; set; }
        public decimal ItemUnitPrice { get; set; }
        public int? CurrencyId { get; set; }
        public string? CurrencyName { get; set; }
    }
}
