using Nop.Core;
using Nop.Core.Domain.Shipping;
using Nop.Core.Plugins;
using Nop.Services.Cms;
using Nop.Services.Shipping;
using Nop.Services.Shipping.Tracking;
using Nop.Web.Framework.Infrastructure;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.Shipping.VendorPostHoc
{
    public class VendorPostHocComputationMethod : BasePlugin, IShippingRateComputationMethod, IWidgetPlugin
    {
        private readonly IWebHelper _webHelper;

        public VendorPostHocComputationMethod(IWebHelper webHelper)
        {
            _webHelper = webHelper;
        }

        public ShippingRateComputationMethodType ShippingRateComputationMethodType => ShippingRateComputationMethodType.Offline;

        public IShipmentTracker ShipmentTracker => null;
        
        public decimal? GetFixedRate(GetShippingOptionRequest getShippingOptionRequest) => null;

        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/VendorPostHoc/Configure";
        }

        public GetShippingOptionResponse GetShippingOptions(GetShippingOptionRequest getShippingOptionRequest)
        {
            return new GetShippingOptionResponse()
            {
                ShippingOptions = new List<ShippingOption>()
                  {
                      new ShippingOption()
                      {
                           Name = "Estimated shipping",
                           Description = "Estimated charges based on the vendors preferences. Can be adjusted after checkout.",
                           ShippingRateComputationMethodSystemName = "Estimated",
                           Rate = 100m
                      }
                  }
            };
        }

        public IList<string> GetWidgetZones()
        {
            return new List<string>()
            {
                AdminWidgetZones.OrderDetailsInfoTop
            };
        }

        public string GetWidgetViewComponentName(string widgetZone)
        {
            switch (widgetZone)
            {
                case string s when s.Equals(AdminWidgetZones.OrderDetailsInfoTop): return "WidgetsVendorOrderInfo";
                default:
                    return null;
            }
        }
    }
}
