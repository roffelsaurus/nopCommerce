using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Plugin.Shipping.VendorPostHoc.Models;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.ExportImport;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Security;
using Nop.Services.Shipping;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.Orders;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Shipping.VendorPostHoc.Controllers
{
    [Area(AreaNames.Admin)]
    [HttpsRequirement(SslRequirement.Yes)]
    [AdminAntiForgery]
    [ValidateIpAddress]
    [AuthorizeAdmin]
    [ValidateVendor]
    public class AdjustOrderShippingController : BasePluginController
    {
        private readonly IOrderService _orderService;
        private readonly IPermissionService _permissionService;
        private readonly IOrderModelFactory _orderModelFactory;
        private readonly IWorkContext _workContext;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly decimal _allowedTotalChange;


        public AdjustOrderShippingController(IOrderService orderService,
            IPermissionService permissionService,
            IOrderModelFactory orderModelFactory,
            IWorkContext workContext,
            ICustomerActivityService customerActivityService,
            ILocalizationService localizationService,
            ILanguageService languageService)//IAddressAttributeParser addressAttributeParser, IAddressService addressService, ICustomerActivityService customerActivityService, IDateTimeHelper dateTimeHelper, IDownloadService downloadService, IEncryptionService encryptionService, IExportManager exportManager, IGiftCardService giftCardService, ILocalizationService localizationService, IOrderModelFactory orderModelFactory, IOrderProcessingService orderProcessingService, IOrderService orderService, IPaymentService paymentService, IPdfService pdfService, IPermissionService permissionService, IPriceCalculationService priceCalculationService, IProductAttributeFormatter productAttributeFormatter, IProductAttributeParser productAttributeParser, IProductAttributeService productAttributeService, IProductService productService, IShipmentService shipmentService, IShippingService shippingService, IShoppingCartService shoppingCartService, IWorkContext workContext, IWorkflowMessageService workflowMessageService, OrderSettings orderSettings) : base(addressAttributeParser, addressService, customerActivityService, dateTimeHelper, downloadService, encryptionService, exportManager, giftCardService, localizationService, orderModelFactory, orderProcessingService, orderService, paymentService, pdfService, permissionService, priceCalculationService, productAttributeFormatter, productAttributeParser, productAttributeService, productService, shipmentService, shippingService, shoppingCartService, workContext, workflowMessageService, orderSettings)
        {
            _orderService = orderService;
            _permissionService = permissionService;
            _orderModelFactory = orderModelFactory;
            _workContext = workContext;
            _customerActivityService = customerActivityService;
            _localizationService = localizationService;
            _languageService = languageService;
            // hardcoded for now based on paypals 115% change allowed
            _allowedTotalChange = 1.15m;
        }

        public IActionResult AdjustOrderShipping(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            //try to get an order with the specified id
            var order = _orderService.GetOrderById(id);
            if (order == null || order.Deleted)
                return RedirectToAction("Edit", "Order", new { id });

            //prepare model
            var model = new AdjustOrderShippingModel(order); // _orderModelFactory.PrepareOrderModel(null, order);

            return View("~/Plugins/Shipping.VendorPostHoc/Views/AdjustOrderShipping.cshtml", model);
        }

        [HttpPost]
        public IActionResult AdjustOrderShipping(int id, AdjustOrderShippingModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            //try to get an order with the specified id
            var order = _orderService.GetOrderById(id);
            if (order == null)
                return RedirectToAction("List");
            
            if (!OrderShippingChangeValidation(order.OrderTotal, order.OrderShippingExclTax, model.OrderShippingExclTaxValue))
            {
                model.Message = string.Format(_localizationService.GetResource(
                    "Plugin.Shipping.VendorPostHoc.AdjustOrderShippingModel.Message.Invalid",
                    _workContext.WorkingLanguage.Id,
                    defaultValue: "Shipping change invalid. Needs to be between {0} and {1}."),
                    0m, _allowedTotalChange);

                return View("~/Plugins/Shipping.VendorPostHoc/Views/AdjustOrderShipping.cshtml", model);
            }
            // vendors can only edit shipping and it is subject to validation.
            order.OrderTotal -= order.OrderShippingExclTax;
            order.OrderShippingExclTax = model.OrderShippingExclTaxValue;
            order.OrderShippingInclTax = order.OrderShippingExclTax;
            order.OrderTotal += order.OrderShippingExclTax;

            //add a note
            order.OrderNotes.Add(new OrderNote
            {
                Note = "Order totals have been edited",
                DisplayToCustomer = false,
                CreatedOnUtc = DateTime.UtcNow
            });
            _orderService.UpdateOrder(order);
            LogEditOrder(order.Id);

            model = new AdjustOrderShippingModel(order);
            model.Message = _localizationService.GetResource("Plugin.Shipping.VendorPostHoc.AdjustOrderShippingModel.Message.Success",
                _workContext.WorkingLanguage.Id,
                defaultValue: "Shipping updated.");
            return View("~/Plugins/Shipping.VendorPostHoc/Views/AdjustOrderShipping.cshtml", model);
        }

        protected void LogEditOrder(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);

            _customerActivityService.InsertActivity("EditOrder",
                string.Format(_localizationService.GetResource("ActivityLog.EditOrder"), order.CustomOrderNumber), order);
        }

        private bool OrderShippingChangeValidation(decimal orderTotal, decimal orderShippingExclTax, decimal orderShippingExclTaxValue)
        {
            var newtotalWithShipping = orderTotal - orderShippingExclTax + orderShippingExclTaxValue;

            var changeOfOldTotal = newtotalWithShipping / orderTotal;

            return orderShippingExclTaxValue >= 0m
                && changeOfOldTotal <= _allowedTotalChange;
        }
    }
}
