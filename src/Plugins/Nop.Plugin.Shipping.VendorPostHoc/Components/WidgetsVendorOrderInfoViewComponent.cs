using Microsoft.AspNetCore.Mvc;
using Nop.Web.Areas.Admin.Models.Orders;
using Nop.Web.Framework.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Shipping.VendorPostHoc.Components
{
    [ViewComponent(Name = PluginWidgets.VendorOrderInfo)]
    public class WidgetsVendorOrderInfoViewComponent : NopViewComponent
    {
        public IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            var ordermodel = additionalData as OrderModel;
            return View("~/Plugins/Shipping.VendorPostHoc/Views/VendorOrderInfo.cshtml", ordermodel);
        }
    }
}
