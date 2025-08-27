using System;
using System.Collections.Generic;

namespace IdentityMicroservice.Entities;

public partial class Carrier
{
    public int CarrierId { get; set; }

    public string? CarrierName { get; set; }

    public string? ContactInfo { get; set; }

    public virtual ICollection<OfferShippingCost> OfferShippingCosts { get; set; } = new List<OfferShippingCost>();
}
