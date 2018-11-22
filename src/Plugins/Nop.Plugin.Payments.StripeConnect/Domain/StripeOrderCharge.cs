using Nop.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Payments.StripeConnect.Domain
{
    public class StripeOrderCharge : BaseEntity
    {
        public int CustomerId { get; set; }
        public int SellerCustomerId { get; set; }
        public Guid OrderGuid { get; set; }
        public string StripeChargeId { get; set; }
    }
}
