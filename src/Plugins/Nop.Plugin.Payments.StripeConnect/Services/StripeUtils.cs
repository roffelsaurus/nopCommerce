using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Payments.StripeConnect.Services
{
    public static class StripeUtils
    {
        public static decimal NopDecimalToStripeDecimal(decimal amount)
        {
            return Math.Floor(amount * 100m); // TODO fix currency to use subcurrency, not 100m
        }

        public static int NopDecimalToStripeInt(decimal amount)
        {
            return Convert.ToInt32(Math.Floor(amount * 100m)); // TODO fix currency to use subcurrency, not 100m
        }
    }
}
