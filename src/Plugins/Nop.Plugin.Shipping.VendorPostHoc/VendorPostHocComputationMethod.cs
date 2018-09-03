using Nop.Core;
using Nop.Core.Domain.Shipping;
using Nop.Core.Plugins;
using Nop.Services.Cms;
using Nop.Services.Localization;
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
        private readonly ILocalizationService _localizationService;

        public VendorPostHocComputationMethod(IWebHelper webHelper, ILocalizationService localizationService)
        {
            _webHelper = webHelper;
            this._localizationService = localizationService;
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
                AdminWidgetZones.OrderDetailsInfoTop,
                AdminWidgetZones.OrderDetailsButtons
            };
        }

        public string GetWidgetViewComponentName(string widgetZone)
        {
            switch (widgetZone)
            {
                case string s when s.Equals(AdminWidgetZones.OrderDetailsInfoTop): return PluginWidgets.VendorOrderInfo;
                case string s when s.Equals(AdminWidgetZones.OrderDetailsButtons): return PluginWidgets.VendorShippingEditBtn;
                default:
                    return null;
            }
        }


        public override void Install()
        {
            _localizationService.AddOrUpdatePluginLocaleResource("Plugin.Shipping.VendorPostHoc.AdjustShipping", "Adjust Shipping");
            base.Install();
        }

        public override void Uninstall()
        {
            _localizationService.DeletePluginLocaleResource("Plugin.Shipping.VendorPostHoc.AdjustShipping");
            base.Uninstall();
        }
    }
}
