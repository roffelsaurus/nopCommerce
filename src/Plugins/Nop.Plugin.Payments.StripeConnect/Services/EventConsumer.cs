using System.Linq;
using Microsoft.AspNetCore.Html;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Payments;
using Nop.Web.Areas.Admin.Models.Customers;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Extensions;
using Nop.Web.Framework.UI;

namespace Nop.Plugin.Payments.StripeConnect.Services
{
    public class EventConsumer :
        IConsumer<PageRenderingEvent>
    {
        private readonly IPaymentService _paymentService;
        
        public EventConsumer(
            IPaymentService paymentService)
        {
            this._paymentService = paymentService;
        }
        
        public void HandleEvent(PageRenderingEvent eventMessage)
        {
            if (eventMessage?.Helper?.ViewContext?.ActionDescriptor == null)
                return;

            //check whether the payment plugin is installed and is active
            var paymentMethod = _paymentService.LoadPaymentMethodBySystemName(StripeConnectConstants.SystemName);
            if (!(paymentMethod?.PluginDescriptor?.Installed ?? false) || !_paymentService.IsPaymentMethodActive(paymentMethod))
                return;

            //add js sсript to one page checkout
            if (eventMessage.GetRouteNames().Any(r => r.Equals("CheckoutOnePage")))
            {
                eventMessage.Helper.AddScriptParts(ResourceLocation.Footer, Javascripts.StripeV3, excludeFromBundle: true);
                eventMessage.Helper.AddCssFileParts("~/Plugins/Payments.StripeConnect/Content/PaymentInfo/paymentinfo.css");

            }
        }
    }
}