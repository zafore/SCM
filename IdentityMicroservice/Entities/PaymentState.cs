using System;
using System.Collections.Generic;

namespace IdentityMicroservice.Entities;

public partial class PaymentState
{
    public int PaymentStatesId { get; set; }

    public string PaymentStatesName { get; set; } = null!;

    public virtual ICollection<OfferContract> OfferContracts { get; set; } = new List<OfferContract>();

    public virtual ICollection<OfferShippingContract> OfferShippingContracts { get; set; } = new List<OfferShippingContract>();
}
