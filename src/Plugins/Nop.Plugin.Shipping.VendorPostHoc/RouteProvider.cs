using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Shipping.VendorPostHoc
{
    public partial class RouteProvider : IRouteProvider
    {
        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="routeBuilder">Route builder</param>
        public void RegisterRoutes(IRouteBuilder routeBuilder)
        {
            //routeBuilder.MapRoute("Plugin.Shipping.VendorPostHoc.AdjustOrderShipping", "Admin/Order/AdjustOrderShipping/{id}",
            //new { controller = "AdjustOrderShipping", action = "AdjustOrderShipping" });
            //routeBuilder.MapRoute("Plugin.Payments.PayPalStandard.PDTHandler", "Admin/PaymentPayPalStandard/PDTHandler",
            //     new { controller = "PaymentPayPalStandard", action = "PDTHandler" });

            //namespace Nop.Plugin.Misc.MyPlugin.Filters
            //    {
            //        public class MyFilterProvider : IFilterProvider
            //        {
            //            private readonly IActionFilter _actionFilter;

            //            public MyFilterProvider(IActionFilter actionFilter)
            //            {
            //                _actionFilter = actionFilter;
            //            }

            //            public IEnumerable<Filter> GetFilters(ControllerContext controllerContext,
            //                ActionDescriptor actionDescriptor)
            //            {
            //                if (actionDescriptor.ControllerDescriptor.ControllerType == typeof(OrderController) &&
            //                    actionDescriptor.ActionName.Equals("AddProductToOrderDetails") &&
            //                    controllerContext.HttpContext.Request.HttpMethod == "POST")
            //                {
            //                    return new Filter[]
            //                    {
            //                    new Filter(_actionFilter, FilterScope.Action, null)
            //                    };
            //                }

            //                return new Filter[] { };
            //            }
            //        }
            //    }


            ////PDT
            //routeBuilder.MapRoute("Plugin.Payments.PayPalStandard.PDTHandler", "Plugins/PaymentPayPalStandard/PDTHandler",
            //     new { controller = "PaymentPayPalStandard", action = "PDTHandler" });

            ////IPN
            //routeBuilder.MapRoute("Plugin.Payments.PayPalStandard.IPNHandler", "Plugins/PaymentPayPalStandard/IPNHandler",
            //     new { controller = "PaymentPayPalStandard", action = "IPNHandler" });

            ////Cancel
            //routeBuilder.MapRoute("Plugin.Payments.PayPalStandard.CancelOrder", "Plugins/PaymentPayPalStandard/CancelOrder",
            //     new { controller = "PaymentPayPalStandard", action = "CancelOrder" });
        }

        /// <summary>
        /// Gets a priority of route provider
        /// </summary>
        public int Priority
        {
            get { return 100; }
        }
    }
}
