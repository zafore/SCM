using System;
using System.Collections.Generic;

namespace IdentityMicroservice.Entities;

public partial class OfferShippingContract
{
    public int OfferShippingContractId { get; set; }

    public int OfferShippingCostId { get; set; }

    public int InstallmentsTypeId { get; set; }

    public decimal? Percentage { get; set; }

    public decimal? Amount { get; set; }

    public int? CurrencyId { get; set; }

    public DateTime? DueDate { get; set; }

    public string? ContractNote { get; set; }

    public int? AttachmentId { get; set; }

    public int? PaymentStatesId { get; set; }

    public virtual Attachment? Attachment { get; set; }

    public virtual Currency? Currency { get; set; }

    public virtual InstallmentsType InstallmentsType { get; set; } = null!;

    public virtual OfferShippingCost OfferShippingCost { get; set; } = null!;

    public virtual PaymentState? PaymentStates { get; set; }
}
