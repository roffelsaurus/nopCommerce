﻿using Microsoft.AspNetCore.Http;
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
using Nop.Plugin.Payments.StripeConnect.Data;
using Nop.Plugin.Payments.StripeConnect.Services;
using System.Linq;
using Nop.Services.Orders;
using Nop.Services.Catalog;
using Nop.Services.Logging;
using Nop.Services.Vendors;
using Nop.Services.Customers;

namespace Nop.Plugin.Payments.StripeConnect
{
    public class StripeConnectPaymentProcessor : BasePlugin, IPaymentMethod, IWidgetPlugin
    {
        private readonly ILocalizationService _localizationservice;
        private readonly IWebHelper _webhelper;
        private readonly ISettingService _settingService;
        private readonly StripeConnectObjectContext _stripeConnectObjectContext;
        private readonly IChargeService _chargeService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IPriceCalculationService _priceCalculationService;
        private readonly ILogger _logger;
        private readonly IVendorService _vendorService;
        private readonly ICustomerService _customerService;
        private readonly IRefundService _refundService;

        public StripeConnectPaymentProcessor(ILocalizationService localizationService,
            IWebHelper webHelper,
            ISettingService settingService,
            StripeConnectObjectContext stripeConnectObjectContext,
            IChargeService chargeService,
            IWorkContext workContext,
            IStoreContext storeContext,
            IPriceCalculationService priceCalculationService,
            ILogger logger,
            IVendorService vendorService,
            ICustomerService customerService,
            IRefundService refundService)
        {
            _localizationservice = localizationService;
            _webhelper = webHelper;
            _settingService = settingService;
            _stripeConnectObjectContext = stripeConnectObjectContext;
            _chargeService = chargeService;
            _workContext = workContext;
            _storeContext = storeContext;
            _priceCalculationService = priceCalculationService;
            _logger = logger;
            _vendorService = vendorService;
            _customerService = customerService;
            _refundService = refundService;
        }

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webhelper.GetStoreLocation()}Admin/StripeConnect/Configure";
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
            _stripeConnectObjectContext.Install();

            _settingService.SaveSetting(new StripeConnectPaymentSettings
            {
                LiveEnvironment = false,
                PublishableKey = "",
                SecretKey = "",
                ClientId = ""
            });
            
            _localizationservice.AddOrUpdatePluginLocaleResource("Plugin.Payments.StripeConnect.Fields.LiveEnvironment", "Live Environment");
            _localizationservice.AddOrUpdatePluginLocaleResource("Plugin.Payments.StripeConnect.Fields.PublishableKey", "Publishable Key");
            _localizationservice.AddOrUpdatePluginLocaleResource("Plugin.Payments.StripeConnect.Fields.SecretKey", "Secret Key");
            _localizationservice.AddOrUpdatePluginLocaleResource("Plugin.Payments.StripeConnect.Fields.ClientId", "Client Id");
            _localizationservice.AddOrUpdatePluginLocaleResource("Plugin.Payments.StripeConnect.AccountNavStripeIntegration",
                "Stripe Onboarding");

            _localizationservice.AddOrUpdatePluginLocaleResource("Plugin.Payments.StripeConnect.OnBoarding.Consent", "By clicking consent you agree to sharing data with ReTrade and Stripe. You will get the option to link your account as a merchant account with Retrade.");
            _localizationservice.AddOrUpdatePluginLocaleResource("Plugin.Payments.StripeConnect.OnBoarding.InvalidToken", "Invalid token");
            _localizationservice.AddOrUpdatePluginLocaleResource("Plugin.Payments.StripeConnect.OnBoarding.Success", "Successful onboarding!");
            _localizationservice.AddOrUpdatePluginLocaleResource("Plugin.Payments.StripeConnect.OnBoarding.Fail", "Error during onboarding.");
            _localizationservice.AddOrUpdatePluginLocaleResource("Plugin.Payments.StripeConnect.OnBoarding.LinkText", "Click here to navigate to Stripe Onboarding");
            _localizationservice.AddOrUpdatePluginLocaleResource("Plugin.Payments.StripeConnect.OnBoarding.AlreadyOnboarded", "You are already onboarded!");

            _localizationservice.AddOrUpdatePluginLocaleResource("Plugin.Payments.StripeConnect.PaymentInfo.PayWithCard", "Pay with card");
            
            base.Install();
        }

        public override void Uninstall()
        {
            _stripeConnectObjectContext.Uninstall();
            _settingService.DeleteSetting<StripeConnectPaymentSettings>();

            _localizationservice.DeletePluginLocaleResource("Plugin.Payments.StripeConnect.AccountNavStripeIntegration");
            _localizationservice.DeletePluginLocaleResource("Plugin.Payments.StripeConnect.Fields.LiveEnvironment");
            _localizationservice.DeletePluginLocaleResource("Plugin.Payments.StripeConnect.Fields.PublishableKey");
            _localizationservice.DeletePluginLocaleResource("Plugin.Payments.StripeConnect.Fields.SecretKey");
            _localizationservice.DeletePluginLocaleResource("Plugin.Payments.StripeConnect.Fields.ClientId");
            
            _localizationservice.DeletePluginLocaleResource("Plugin.Payments.StripeConnect.OnBoarding.Consent");
            _localizationservice.DeletePluginLocaleResource("Plugin.Payments.StripeConnect.OnBoarding.InvalidToken");
            _localizationservice.DeletePluginLocaleResource("Plugin.Payments.StripeConnect.OnBoarding.Success");
            _localizationservice.DeletePluginLocaleResource("Plugin.Payments.StripeConnect.OnBoarding.Fail");
            _localizationservice.DeletePluginLocaleResource("Plugin.Payments.StripeConnect.OnBoarding.LinkText");
            _localizationservice.DeletePluginLocaleResource("Plugin.Payments.StripeConnect.OnBoarding.AlreadyOnboarded");
            _localizationservice.DeletePluginLocaleResource("Plugin.Payments.StripeConnect.PaymentInfo.PayWithCard");
            
            base.Uninstall();
        }
        #endregion

        #region Payment Methods

        public bool SupportCapture => false;

        public bool SupportPartiallyRefund => false;

        public bool SupportRefund => true;

        public bool SupportVoid => false;

        public RecurringPaymentType RecurringPaymentType => RecurringPaymentType.NotSupported;

        public PaymentMethodType PaymentMethodType => PaymentMethodType.Standard;

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
            var cart = _workContext.CurrentCustomer.ShoppingCartItems
            .Where(sci => sci.ShoppingCartType == ShoppingCartType.ShoppingCart)
            .LimitPerStore(_storeContext.CurrentStore.Id)
            .ToList();

            var vendorids = cart
                .Select(i => i.Product.VendorId)
            .Distinct();

            if (vendorids.Count() != 1)
                return null;

            var customer = _customerService.GetAllCustomers(vendorId: vendorids.First());

            if (customer.Count() != 1)
                return null;

            decimal subTotal = decimal.Zero;
            foreach (var shoppingCartItem in cart)
            {
                subTotal += _priceCalculationService.GetSubTotal(shoppingCartItem, true);
            }
            var processPaymentRequest = new ProcessPaymentRequest();

            var tokenfound = form.TryGetValue(PaymentInfoFormKeys.Token, out var token);

            processPaymentRequest.CustomValues = new Dictionary<string, object>()
                {
                    [PaymentInfoFormKeys.Token] = token.ToString(),
                    [PaymentInfoFormKeys.SellerCustomerId] = customer.First().Id,
                    [PaymentInfoFormKeys.OrderSubTotal] = subTotal
                };
            return processPaymentRequest;
        }

        public string GetPublicViewComponentName()
        {
            return ViewComponents.PaymentInfo;
        }
        
        public bool HidePaymentMethod(IList<ShoppingCartItem> cart)
        {
            return false;
        }

        public void PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            throw new NotImplementedException();
        }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            var token = (string)processPaymentRequest.CustomValues[PaymentInfoFormKeys.Token];

            // todo error handling
            _logger.Information("token: " + token);

            var sellerCustomerId = Convert.ToInt32(processPaymentRequest.CustomValues[PaymentInfoFormKeys.SellerCustomerId]);
            var subTotal = Convert.ToDecimal(processPaymentRequest.CustomValues[PaymentInfoFormKeys.OrderSubTotal]);
            return _chargeService.Charge(token, subTotal, sellerCustomerId,
                processPaymentRequest);
        }

        public ProcessPaymentResult ProcessRecurringPayment(ProcessPaymentRequest processPaymentRequest)
        {
            throw new NotImplementedException();
        }

        public RefundPaymentResult Refund(RefundPaymentRequest refundPaymentRequest)
        {
            return _refundService.Refund(refundPaymentRequest);
        }

        public IList<string> ValidatePaymentForm(IFormCollection form)
        {
            return new List<string>();
        }

        public VoidPaymentResult Void(VoidPaymentRequest voidPaymentRequest)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
