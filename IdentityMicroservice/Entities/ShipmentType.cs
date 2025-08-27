using System;
using System.Collections.Generic;

namespace IdentityMicroservice.Entities;

public partial class ShipmentType
{
    public int ShipmentTypeId { get; set; }

    public string? ShipmentTypeName { get; set; }

    public virtual ICollection<OfferShippingCost> OfferShippingCosts { get; set; } = new List<OfferShippingCost>();
}
