using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Payments.StripeConnect.Models
{
    public class PaymentInfo
    {
        public PaymentInfo()
        {
           // ExpireMonths = new List<SelectListItem>();
          //  ExpireYears = new List<SelectListItem>();
        }
        public string Token { get; set; }
        //public string CardName { get; set; }
        //public string CardNumber { get; set; }

        //public string CardCode { get; set; }

        //public string ExpireMonth { get; set; }
        //public IList<SelectListItem> ExpireMonths { get; set; }

        //public string ExpireYear { get; set; }
        //public IList<SelectListItem> ExpireYears { get; set; }


    }
}
