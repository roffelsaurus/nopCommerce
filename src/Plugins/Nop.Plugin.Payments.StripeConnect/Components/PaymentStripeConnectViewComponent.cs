using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Payments.StripeConnect.Models;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Payments.StripeConnect.Components
{
    [ViewComponent(Name = ViewComponents.PaymentInfo)]
    public class PaymentStripeConnectViewComponent : NopViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var model = new PaymentInfo();
            
            return View("~/Plugins/Payments.StripeConnect/Views/PaymentInfo.cshtml", model);
        }
    }
}
