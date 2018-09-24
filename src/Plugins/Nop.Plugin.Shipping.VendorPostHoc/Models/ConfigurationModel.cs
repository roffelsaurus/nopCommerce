
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Shipping.VendorPostHoc.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        [NopResourceDisplayName("Plugin.Shipping.VendorPostHoc.Configure.AllowedTotalShippingCostChange")]
        public decimal AllowedTotalShippingCostChange { get; set; }
        public ConfigurationModel()
        {
        }
    }
}