using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suppliers.Api.Models
{
    [Table("OfferContract")]
    public class OfferContract
    {
        [Key]
        [Column("OfferContractId")]
        public int OfferContractId { get; set; }

        [Required]
        [Column("OfferID")]
        public int OfferID { get; set; }

        [Required]
        [Column("InstallmentsTypeId")]
        public int InstallmentsTypeId { get; set; }

        [Required]
        [Column("Percentage", TypeName = "decimal(18,2)")]
        public decimal Percentage { get; set; }

        [Required]
        [Column("Amount", TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [Column("CurrencyId")]
        public int CurrencyId { get; set; }

        [Column("DueDate")]
        public DateTime? DueDate { get; set; }

        [Column("ContractNote")]
        public string? ContractNote { get; set; }

        [Column("AttachmentId")]
        public int? AttachmentId { get; set; }

        [Column("PaymentStatesId")]
        public int? PaymentStatesId { get; set; }

        // Navigation properties
        [ForeignKey("OfferID")]
        public virtual Offer Offer { get; set; } = null!;

        [ForeignKey("InstallmentsTypeId")]
        public virtual InstallmentsType InstallmentsType { get; set; } = null!;

        [ForeignKey("CurrencyId")]
        public virtual Currency Currency { get; set; } = null!;

        [ForeignKey("AttachmentId")]
        public virtual Attachment? Attachment { get; set; }

        [ForeignKey("PaymentStatesId")]
        public virtual PaymentState? PaymentState { get; set; }
    }
}
