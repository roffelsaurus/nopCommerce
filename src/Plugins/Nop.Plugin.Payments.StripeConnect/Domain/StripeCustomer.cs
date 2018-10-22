using Nop.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Payments.StripeConnect.Domain
{
    public class StripeCustomer : BaseEntity
    {
        public int CustomerId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string StripeUserId { get; set; }
        public string StripePublishableKey { get; set; }
    }
    
}
