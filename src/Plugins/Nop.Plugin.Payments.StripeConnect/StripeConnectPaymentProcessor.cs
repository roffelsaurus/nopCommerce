using Microsoft.AspNetCore.Http;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Core.Plugins;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Payments;
using Nop.Web.Framework.Infrastructure;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.Payments.StripeConnect
{
    public class StripeConnectPaymentProcessor : BasePlugin, IPaymentMethod, IWidgetPlugin
    {
        private readonly ILocalizationService _localizationservice;
        private readonly IWebHelper _webhelper;
        private readonly ISettingService _settingService;

        public StripeConnectPaymentProcessor(ILocalizationService localizationService,
            IWebHelper webHelper,
            ISettingService settingService)
        {
            _localizationservice = localizationService;
            _webhelper = webHelper;
            _settingService = settingService;
        }

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webhelper.GetStoreLocation()}Admin/PaymentStripeConnect/Configure";
        }

        #region WidgetPlugin Methods
        public string GetWidgetViewComponentName(string widgetZone)
        {
            switch (widgetZone)
            {
                case string s when s.Equals(PublicWidgetZones.AccountNavigationBefore): return PluginWidgets.AccountNavStripeIntegration;
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

            _settingService.SaveSetting(new StripeConnectPaymentSettings
            {
                LiveEnvironment = false,
                PublishableKey = "",
                SecretKey = ""
            });


            _localizationservice.AddOrUpdatePluginLocaleResource("Plugin.Payments.StripeConnect.Fields.LiveEnvironment", "Live Environment");
            _localizationservice.AddOrUpdatePluginLocaleResource("Plugin.Payments.StripeConnect.Fields.PublishableKey", "Publishable Key");
            _localizationservice.AddOrUpdatePluginLocaleResource("Plugin.Payments.StripeConnect.Fields.SecretKey", "Secret Key");
            _localizationservice.AddOrUpdatePluginLocaleResource("Plugin.Payments.StripeConnect.AccountNavStripeIntegration",
                "Stripe Onboarding");

            _localizationservice.AddOrUpdatePluginLocaleResource("Plugin.Payments.StripeConnect.OnBoarding.Consent", "By clicking consent you agree to sharing data with ReTrade and Stripe. You will get the option to link your account as a merchant account with Retrade.");
            _localizationservice.AddOrUpdatePluginLocaleResource("Plugin.Payments.StripeConnect.OnBoarding.ConsentAction", "Consent");
            _localizationservice.AddOrUpdatePluginLocaleResource("Plugin.Payments.StripeConnect.OnBoarding.StripeMiniBrowerAction", "Open Stripe Mini Browser");

            base.Install();
        }

        public override void Uninstall()
        {
            //_vendorPostHocObjectContext.Uninstall();
            _settingService.DeleteSetting<StripeConnectPaymentSettings>();

            _localizationservice.DeletePluginLocaleResource("Plugin.Payments.StripeConnect.AccountNavStripeIntegration");
            _localizationservice.DeletePluginLocaleResource("Plugin.Payments.StripeConnect.Fields.LiveEnvironment");
            _localizationservice.DeletePluginLocaleResource("Plugin.Payments.StripeConnect.Fields.PublishableKey");
            _localizationservice.DeletePluginLocaleResource("Plugin.Payments.StripeConnect.Fields.SecretKey");

            _localizationservice.DeletePluginLocaleResource("Plugin.Payments.StripeConnect.OnBoarding.Consent");
            _localizationservice.DeletePluginLocaleResource("Plugin.Payments.StripeConnect.OnBoarding.ConsentAction");
            _localizationservice.DeletePluginLocaleResource("Plugin.Payments.StripeConnect.OnBoarding.StripeMiniBrowerAction");
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

        public string PaymentMethodDescription => "StripeConnect";

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
