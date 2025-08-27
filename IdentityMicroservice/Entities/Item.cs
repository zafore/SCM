using System;
using System.Collections.Generic;

namespace IdentityMicroservice.Entities;

public partial class Item
{
    public int ItemId { get; set; }

    public string? ItemName { get; set; }

    public string? ItemCode { get; set; }

    public string? ItemBarCode { get; set; }

    public virtual ICollection<OfferItem> OfferItems { get; set; } = new List<OfferItem>();
}
