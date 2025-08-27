using System;
using System.Collections.Generic;

namespace IdentityMicroservice.Entities;

public partial class Currency
{
    public int CurrencyId { get; set; }

    public string CurrencyName { get; set; } = null!;

    public string? CurrencyFlag { get; set; }

    public virtual ICollection<OfferContract> OfferContracts { get; set; } = new List<OfferContract>();

    public virtual ICollection<OfferItem> OfferItems { get; set; } = new List<OfferItem>();

    public virtual ICollection<OfferShippingContract> OfferShippingContracts { get; set; } = new List<OfferShippingContract>();
}
