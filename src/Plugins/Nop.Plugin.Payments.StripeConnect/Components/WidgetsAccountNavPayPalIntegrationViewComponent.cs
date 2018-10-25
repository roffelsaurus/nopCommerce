using Microsoft.AspNetCore.Mvc;
using Nop.Web.Factories;
using Nop.Web.Areas.Admin.Models.Orders;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Customer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Payments.StripeConnect.Components
{
    [ViewComponent(Name = PluginWidgets.AccountNavStripeIntegration)]
    public class WidgetsAccountNavStripeIntegrationViewComponent : NopViewComponent
    {
        public IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            return View("~/Plugins/Payments.StripeConnect/Views/AccountNavStripeIntegration.cshtml");
        }
    }
}
