using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suppliers.Api.Models
{
    [Table("OfferShippingContract")]
    public class OfferShippingContract
    {
        [Key]
        [Column("OfferShippingContractId")]
        public int OfferShippingContractId { get; set; }

        [Required]
        [Column("OfferShippingCostId")]
        public int OfferShippingCostId { get; set; }

        [Required]
        [Column("InstallmentsTypeId")]
        public int InstallmentsTypeId { get; set; }

        [Column("Percentage", TypeName = "decimal(18,2)")]
        public decimal? Percentage { get; set; }

        [Column("Amount", TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }

        [Column("CurrencyId")]
        public int? CurrencyId { get; set; }

        [Column("DueDate")]
        public DateTime? DueDate { get; set; }

        [Column("ContractNote")]
        public string? ContractNote { get; set; }

        [Column("AttachmentId")]
        public int? AttachmentId { get; set; }

        [Column("PaymentStatesId")]
        public int? PaymentStatesId { get; set; }

        // Navigation properties
        [ForeignKey("OfferShippingCostId")]
        public virtual OfferShippingCost OfferShippingCost { get; set; } = null!;

        [ForeignKey("InstallmentsTypeId")]
        public virtual InstallmentsType InstallmentsType { get; set; } = null!;

        [ForeignKey("CurrencyId")]
        public virtual Currency? Currency { get; set; }

        [ForeignKey("AttachmentId")]
        public virtual Attachment? Attachment { get; set; }

        [ForeignKey("PaymentStatesId")]
        public virtual PaymentState? PaymentState { get; set; }
    }
}
