using System;
using System.Collections.Generic;

namespace IdentityMicroservice.Entities;

public partial class Attachment
{
    public int AttachmentId { get; set; }

    public string AttachmentForTable { get; set; } = null!;

    public string AttachmentName { get; set; } = null!;

    public DateTime? AttachmentDate { get; set; }

    public virtual ICollection<AttachmentFile> AttachmentFiles { get; set; } = new List<AttachmentFile>();

    public virtual ICollection<OfferContract> OfferContracts { get; set; } = new List<OfferContract>();

    public virtual ICollection<OfferItem> OfferItems { get; set; } = new List<OfferItem>();

    public virtual ICollection<OfferShippingContract> OfferShippingContracts { get; set; } = new List<OfferShippingContract>();

    public virtual ICollection<OfferShippingCost> OfferShippingCosts { get; set; } = new List<OfferShippingCost>();

    public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();

    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
}
