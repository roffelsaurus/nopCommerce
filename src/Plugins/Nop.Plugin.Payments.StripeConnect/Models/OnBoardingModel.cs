using Nop.Web.Models.Customer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Payments.StripeConnect.Models
{
    public class OnBoardingModel
    {
        public string OnBoardingUrl { get; set; }
        public bool AlreadyOnboard { get; set; }
    }
}
