﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Payments.StripeConnect
{
    public static class PluginWidgets
    {
        public const string AccountNavStripeIntegration = "WidgetsAccountNavStripeIntegration";
    }

    public static class ViewComponents
    {
        public const string PaymentInfo = "PaymentStripeConnect";
    }

    public static class PaymentInfoFormKeys
    {
        public const string Token = "Token";
        public const string SellerCustomerId = "SellerCustomerId";
        public const string OrderSubTotal = "OrderSubTotal";
    }

    public static class StripeConnectConstants
    {
        public const string SystemName = "Payments.StripeConnect";
    }

    public static class Javascripts
    {
        public const string StripeV3 = "https://js.stripe.com/v3/";
    }
}