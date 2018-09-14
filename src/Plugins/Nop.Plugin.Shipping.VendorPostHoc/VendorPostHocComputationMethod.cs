using Nop.Core;
using Nop.Core.Domain.Shipping;
using Nop.Core.Plugins;
using Nop.Services.Cms;
using Nop.Services.Configuration;
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
        public const string SHIPPINGRATECOMPUTATIONMETHODSYSTEMNAME = "Baseline";
        private readonly IWebHelper _webHelper;
        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;
        private readonly VendorPostHocSettings _vendorPostHocSettings;

        public VendorPostHocComputationMethod(IWebHelper webHelper, 
            ILocalizationService localizationService,
            ISettingService settingService,
            VendorPostHocSettings vendorPostHocSettings)
        {
            _webHelper = webHelper;
            _localizationService = localizationService;
            _settingService = settingService;
            _vendorPostHocSettings = vendorPostHocSettings;
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
                           Name = _localizationService.GetResource("Plugin.Shipping.VendorPostHoc.ShippingOptions.Name"),
                           Description = _localizationService.GetResource("Plugin.Shipping.VendorPostHoc.ShippingOptions.Description"),
                           ShippingRateComputationMethodSystemName = SHIPPINGRATECOMPUTATIONMETHODSYSTEMNAME,
                           Rate = _vendorPostHocSettings.ShippingCost
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
            var settings = new VendorPostHocSettings()
            {
                ShippingCost = 0
            };
            _settingService.SaveSetting(settings);

            _localizationService.AddOrUpdatePluginLocaleResource("Plugin.Shipping.VendorPostHoc.ShippingOptions.Name",
                "Baseline shipping");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugin.Shipping.VendorPostHoc.ShippingOptions.Description",
                "Baseline charges based on the vendors preferences. Can be adjusted after checkout.");

            _localizationService.AddOrUpdatePluginLocaleResource("Plugin.Shipping.VendorPostHoc.Configure.ShippingCost", "Checkout Shipping cost");
            
            _localizationService.AddOrUpdatePluginLocaleResource("Plugin.Shipping.VendorPostHoc.AdjustShipping", "Adjust Shipping");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugin.Shipping.VendorPostHoc.AdjustOrderShippingModel.Message.Invalid",
                "Shipping change invalid. Needs to be between {0} and {1}.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugin.Shipping.VendorPostHoc.AdjustOrderShippingModel.Message.Success",
                "Shipping updated.");



            base.Install();
        }

        public override void Uninstall()
        {
            _settingService.DeleteSetting<VendorPostHocSettings>();

            _localizationService.DeletePluginLocaleResource("Plugin.Shipping.VendorPostHoc.ShippingOptions.Name");
            _localizationService.DeletePluginLocaleResource("Plugin.Shipping.VendorPostHoc.ShippingOptions.Description");

            _localizationService.DeletePluginLocaleResource("Plugin.Shipping.VendorPostHoc.Configure.ShippingCost");

            _localizationService.DeletePluginLocaleResource("Plugin.Shipping.VendorPostHoc.AdjustShipping");
            _localizationService.DeletePluginLocaleResource("Plugin.Shipping.VendorPostHoc.AdjustOrderShippingModel.Message.Invalid");
            _localizationService.DeletePluginLocaleResource("Plugin.Shipping.VendorPostHoc.AdjustOrderShippingModel.Message.Success");

            base.Uninstall();
        }
    }
}
