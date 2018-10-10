using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Payments.PayPalMarketplace.Models
{
    public class ConfigurationModel
    {
        [NopResourceDisplayName("Plugins.Payments.PayPalMarketPlace.Fields.UseSandbox")]
        public bool UseSandbox { get; set; }

        [NopResourceDisplayName("Plugins.Payments.PayPalMarketPlace.Fields.ClientId")]
        public string ClientId { get; set; }

        [NopResourceDisplayName("Plugins.Payments.PayPalMarketPlace.Fields.ClientSecret")]
        public string ClientSecret { get; set; }
    }
}
