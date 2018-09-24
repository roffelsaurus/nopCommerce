using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Shipping;
using Nop.Core.Plugins;
using Nop.Plugin.Shipping.VendorPostHoc.Data;
using Nop.Plugin.Shipping.VendorPostHoc.Domain;
using Nop.Plugin.Shipping.VendorPostHoc.Services;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Shipping;
using Nop.Services.Shipping.Tracking;
using Nop.Web.Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.Shipping.VendorPostHoc
{
    public class VendorPostHocComputationMethod : BasePlugin, IShippingRateComputationMethod, IWidgetPlugin
    {
        public const string SHIPPINGRATECOMPUTATIONMETHODSYSTEMNAME = "Baseline";
        private readonly IWebHelper _webHelper;
        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;
        private readonly VendorPostHocObjectContext _vendorPostHocObjectContext;
        private readonly IVendorConfigurationService _vendorconfigurationservice;

        public VendorPostHocComputationMethod(IWebHelper webHelper, 
            ILocalizationService localizationService,
            ISettingService settingService,
            VendorPostHocObjectContext vendorPostHocObjectContext,
            IVendorConfigurationService vendorConfigurationService)
        {
            _webHelper = webHelper;
            _localizationService = localizationService;
            _settingService = settingService;
            _vendorPostHocObjectContext = vendorPostHocObjectContext;
            _vendorconfigurationservice = vendorConfigurationService;
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
            var vendorids = getShippingOptionRequest.Items
                .Select(i => i.ShoppingCartItem.Product.VendorId)
                .Distinct();
            if (vendorids.Count() != 1)
                return null;

            var vendorconfig = _vendorconfigurationservice.GetForVendor(vendorids.First());

            return new GetShippingOptionResponse()
            {
                ShippingOptions = new List<ShippingOption>()
                  {
                      new ShippingOption()
                      {
                           Name = _localizationService.GetResource("Plugin.Shipping.VendorPostHoc.ShippingOptions.Name"),
                           Description = _localizationService.GetResource("Plugin.Shipping.VendorPostHoc.ShippingOptions.Description"),
                           ShippingRateComputationMethodSystemName = SHIPPINGRATECOMPUTATIONMETHODSYSTEMNAME,
                           Rate = vendorconfig.ShippingCost
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
            _vendorPostHocObjectContext.Install();
            var settings = new VendorPostHocSettings()
            {
                AllowedTotalShippingCostChange = 1.15m // default paypal allowed change
            };
            _settingService.SaveSetting(settings);

            _localizationService.AddOrUpdatePluginLocaleResource("Plugin.Shipping.VendorPostHoc.ShippingOptions.Name",
                "Baseline shipping");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugin.Shipping.VendorPostHoc.ShippingOptions.Description",
                "Baseline charges based on the vendors preferences. Can be adjusted after checkout.");

            _localizationService.AddOrUpdatePluginLocaleResource("Plugin.Shipping.VendorPostHoc.Configure.AllowedTotalShippingCostChange", 
                "Allowed total shipping cost change(%)");

            _localizationService.AddOrUpdatePluginLocaleResource("Plugin.Shipping.VendorPostHoc.VendorConfiguration.ShippingCost",
                "Standard shipping cost");
            


            _localizationService.AddOrUpdatePluginLocaleResource("Plugin.Shipping.VendorPostHoc.AdjustShipping", "Adjust Shipping");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugin.Shipping.VendorPostHoc.AdjustOrderShippingModel.Message.Invalid",
                "Shipping change invalid. Needs to be between {0} and {1}.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugin.Shipping.VendorPostHoc.AdjustOrderShippingModel.Message.Success",
                "Shipping updated.");



            base.Install();
        }

        public override void Uninstall()
        {
            _vendorPostHocObjectContext.Uninstall();
            _settingService.DeleteSetting<VendorPostHocSettings>();

            _localizationService.DeletePluginLocaleResource("Plugin.Shipping.VendorPostHoc.ShippingOptions.Name");
            _localizationService.DeletePluginLocaleResource("Plugin.Shipping.VendorPostHoc.ShippingOptions.Description");

            _localizationService.DeletePluginLocaleResource("Plugin.Shipping.VendorPostHoc.Configure.AllowedTotalShippingCostChange");

            _localizationService.DeletePluginLocaleResource("Plugin.Shipping.VendorPostHoc.VendorConfiguration.ShippingCost");
            
            _localizationService.DeletePluginLocaleResource("Plugin.Shipping.VendorPostHoc.AdjustShipping");
            _localizationService.DeletePluginLocaleResource("Plugin.Shipping.VendorPostHoc.AdjustOrderShippingModel.Message.Invalid");
            _localizationService.DeletePluginLocaleResource("Plugin.Shipping.VendorPostHoc.AdjustOrderShippingModel.Message.Success");

            base.Uninstall();
        }
    }
}
