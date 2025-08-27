using System;
using System.Collections.Generic;

namespace IdentityMicroservice.Entities;

public partial class OfferShippingCost
{
    public int OfferShippingCostId { get; set; }

    public int CarrierId { get; set; }

    public int ShipmentTypeId { get; set; }

    public decimal ShippingCost { get; set; }

    public int CurrencyId { get; set; }

    public string? ShippingCostNote { get; set; }

    public int? AttachmentId { get; set; }

    public int OfferId { get; set; }

    public virtual Attachment? Attachment { get; set; }

    public virtual Carrier Carrier { get; set; } = null!;

    public virtual Offer Offer { get; set; } = null!;

    public virtual ICollection<OfferShippingContract> OfferShippingContracts { get; set; } = new List<OfferShippingContract>();

    public virtual ShipmentType ShipmentType { get; set; } = null!;
}
