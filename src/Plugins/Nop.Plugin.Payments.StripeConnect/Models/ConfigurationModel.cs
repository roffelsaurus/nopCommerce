using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Payments.StripeConnect.Models
{
    public class ConfigurationModel
    {

        [NopResourceDisplayName("Plugin.Payments.StripeConnect.Fields.LiveEnvironment")]
        public bool LiveEnvironment { get; set; }

        [NopResourceDisplayName("Plugin.Payments.StripeConnect.Fields.SecretKey")]
        public string SecretKey { get; set; }

        [NopResourceDisplayName("Plugin.Payments.StripeConnect.Fields.PublishableKey")]
        public string PublishableKey { get; set; }


        [NopResourceDisplayName("Plugin.Payments.StripeConnect.Fields.ClientId")]
        public string ClientId { get; set; }
    }
}
