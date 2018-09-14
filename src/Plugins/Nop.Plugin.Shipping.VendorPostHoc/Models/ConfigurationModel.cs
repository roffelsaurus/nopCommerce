
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Shipping.VendorPostHoc.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        [NopResourceDisplayName("Plugin.Shipping.VendorPostHoc.Configure.ShippingCost")]
        public decimal ShippingCost { get; set; }
        public ConfigurationModel()
        {
        }
    }
}