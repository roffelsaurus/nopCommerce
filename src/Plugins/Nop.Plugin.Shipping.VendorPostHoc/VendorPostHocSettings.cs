using Nop.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Shipping.VendorPostHoc
{
    public class VendorPostHocSettings : ISettings
    {
        public decimal AllowedTotalShippingCostChange { get; set; }

        public VendorPostHocSettings()
        {

        }
    }
}
