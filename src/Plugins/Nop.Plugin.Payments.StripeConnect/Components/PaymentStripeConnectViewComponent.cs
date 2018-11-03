using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Payments.StripeConnect.Models;
using Nop.Web.Framework.Components;
using System;

namespace Nop.Plugin.Payments.StripeConnect.Components
{
    [ViewComponent(Name = ViewComponents.PaymentInfo)]
    public class PaymentStripeConnectViewComponent : NopViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var model = new PaymentInfo();
            //prepare years
            //for (var i = 0; i < 15; i++)
            //{
            //    var year = (DateTime.Now.Year + i).ToString();
            //    model.ExpireYears.Add(new SelectListItem { Text = year, Value = year, });
            //}

            ////prepare months
            //for (var i = 1; i <= 12; i++)
            //{
            //    model.ExpireMonths.Add(new SelectListItem { Text = i.ToString("D2"), Value = i.ToString(), });
            //}
            return View("~/Plugins/Payments.StripeConnect/Views/PaymentInfo.cshtml", model);
        }
    }
}
