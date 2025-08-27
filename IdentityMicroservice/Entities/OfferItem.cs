using System;
using System.Collections.Generic;

namespace IdentityMicroservice.Entities;

public partial class OfferItem
{
    public int OfferItemId { get; set; }

    public int OfferId { get; set; }

    public int ItemId { get; set; }

    public decimal? ItemQuantity { get; set; }

    public decimal? ItemUnitPrice { get; set; }

    public int? CurrencyId { get; set; }

    public int? PrmotionFileAttachmentId { get; set; }

    public virtual Currency? Currency { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual Offer Offer { get; set; } = null!;

    public virtual Attachment? PrmotionFileAttachment { get; set; }
}
