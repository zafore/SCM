using System;
using System.Collections.Generic;

namespace IdentityMicroservice.Entities;

public partial class Offer
{
    public int OfferId { get; set; }

    public string? OfferName { get; set; }

    public string? OfferDescription { get; set; }

    public DateTime? OfferDate { get; set; }

    public int? OfferStatusId { get; set; }

    public int? SupplierId { get; set; }

    public int? AttachmentId { get; set; }

    public virtual Attachment? Attachment { get; set; }

    public virtual ICollection<OfferContract> OfferContracts { get; set; } = new List<OfferContract>();

    public virtual ICollection<OfferItem> OfferItems { get; set; } = new List<OfferItem>();

    public virtual ICollection<OfferShippingCost> OfferShippingCosts { get; set; } = new List<OfferShippingCost>();

    public virtual OfferStatus? OfferStatus { get; set; }

    public virtual Supplier? Supplier { get; set; }
}
