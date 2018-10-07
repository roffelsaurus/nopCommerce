using Microsoft.AspNetCore.Http;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Core.Plugins;
using Nop.Services.Cms;
using Nop.Services.Localization;
using Nop.Services.Payments;
using Nop.Web.Framework.Infrastructure;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.Payments.PayPalMarketplace
{
    public class PayPalMarketPlacePaymentProcessor : BasePlugin, IPaymentMethod, IWidgetPlugin
    {
        private readonly ILocalizationService _localizationservice;
        private readonly IWebHelper _webhelper;

        public PayPalMarketPlacePaymentProcessor(ILocalizationService localizationService,
            IWebHelper webHelper)
        {
            _localizationservice = localizationService;
            _webhelper = webHelper;
        }

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webhelper.GetStoreLocation()}Admin/PaymentPayPalMarketPlace/Configure";
        }

        #region WidgetPlugin Methods
        public string GetWidgetViewComponentName(string widgetZone)
        {
            switch (widgetZone)
            {
                case string s when s.Equals(PublicWidgetZones.AccountNavigationBefore): return PluginWidgets.AccountNavPayPalIntegration;
                default:
                    return null;
            }
        }

        public IList<string> GetWidgetZones()
        {
            return new List<string>()
            {
                PublicWidgetZones.AccountNavigationBefore
            };
        }

        #endregion

        #region Install
        public override void Install()
        {
            //_vendorPostHocObjectContext.Install();
            //var settings = new VendorPostHocSettings()
            //{
            //    AllowedTotalShippingCostChange = 1.15m // default paypal allowed change
            //};
            //_settingService.SaveSetting(settings);

            _localizationservice.AddOrUpdatePluginLocaleResource("Plugin.Payments.PayPalMarketPlace.AccountNavPayPalIntegration",
                "PayPal Onboarding");

            base.Install();
        }

        public override void Uninstall()
        {
            //_vendorPostHocObjectContext.Uninstall();
            //_settingService.DeleteSetting<VendorPostHocSettings>();

            _localizationservice.DeletePluginLocaleResource("Plugin.Payments.PayPalMarketPlace.AccountNavPayPalIntegration");
            base.Uninstall();
        }
        #endregion

        #region Payment Methods

        public bool SupportCapture => true;

        public bool SupportPartiallyRefund => false;

        public bool SupportRefund => true;

        public bool SupportVoid => true;

        public RecurringPaymentType RecurringPaymentType => RecurringPaymentType.NotSupported;

        public PaymentMethodType PaymentMethodType => PaymentMethodType.Redirection;

        public bool SkipPaymentInfo => false;

        public string PaymentMethodDescription => "PayPalMarketPlace";

        public CancelRecurringPaymentResult CancelRecurringPayment(CancelRecurringPaymentRequest cancelPaymentRequest)
        {
            throw new NotImplementedException();
        }

        public bool CanRePostProcessPayment(Order order)
        {
            throw new NotImplementedException();
        }

        public CapturePaymentResult Capture(CapturePaymentRequest capturePaymentRequest)
        {
            throw new NotImplementedException();
        }

        public decimal GetAdditionalHandlingFee(IList<ShoppingCartItem> cart)
        {
            return 0m;
        }

        public ProcessPaymentRequest GetPaymentInfo(IFormCollection form)
        {
            throw new NotImplementedException();
        }

        public string GetPublicViewComponentName()
        {
            throw new NotImplementedException();
        }
        
        public bool HidePaymentMethod(IList<ShoppingCartItem> cart)
        {
            throw new NotImplementedException();
        }

        public void PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            throw new NotImplementedException();
        }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            throw new NotImplementedException();
        }

        public ProcessPaymentResult ProcessRecurringPayment(ProcessPaymentRequest processPaymentRequest)
        {
            throw new NotImplementedException();
        }

        public RefundPaymentResult Refund(RefundPaymentRequest refundPaymentRequest)
        {
            throw new NotImplementedException();
        }

        public IList<string> ValidatePaymentForm(IFormCollection form)
        {
            throw new NotImplementedException();
        }

        public VoidPaymentResult Void(VoidPaymentRequest voidPaymentRequest)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
