using Microsoft.AspNetCore.Mvc;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.Orders;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Customer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Payments.PayPalMarketplace.Components
{
    [ViewComponent(Name = PluginWidgets.AccountNavPayPalIntegration)]
    public class WidgetsAccountNavPayPalIntegrationViewComponent : NopViewComponent
    {
        public IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            var model = additionalData as CustomerNavigationModel;
            return View("~/Plugins/Payments.PayPalMarketplace/Views/AccountNavPayPalIntegration.cshtml", model);
        }
    }
}
