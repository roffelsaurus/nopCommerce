
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Shipping.VendorPostHoc.Models
{
    public class VendorConfigurationModel : BaseNopModel
    {
        [NopResourceDisplayName("Plugin.Shipping.VendorPostHoc.VendorConfiguration.ShippingCost")]
        public decimal ShippingCost { get; set; }
        public VendorConfigurationModel()
        {
        }
    }
}