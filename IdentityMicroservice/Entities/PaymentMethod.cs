using System;
using System.Collections.Generic;

namespace IdentityMicroservice.Entities;

public partial class PaymentMethod
{
    public int PaymentMethodId { get; set; }

    public string PaymentMethodName { get; set; } = null!;
}
