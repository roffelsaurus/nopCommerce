using System;
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
    }
}
