using Nop.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Shipping.VendorPostHoc.Domain
{
    public class VendorConfiguration : BaseEntity 
    {
        public int VendorId { get; set; }
        
        public decimal ShippingCost { get; set; }
    }
}
