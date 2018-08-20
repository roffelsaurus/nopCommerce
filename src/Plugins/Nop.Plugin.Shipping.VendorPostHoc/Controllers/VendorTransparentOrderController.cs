using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Orders;
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
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Shipping.VendorPostHoc.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public class VendorTransparentOrderController : BasePluginController
    {
        private readonly IOrderService _orderService;
        private readonly IPermissionService _permissionService;
        private readonly IOrderModelFactory _orderModelFactory;
        private readonly IWorkContext _workContext;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILocalizationService _localizationService;

        private readonly decimal _allowedTotalChange;


        public VendorTransparentOrderController(IOrderService orderService,
            IPermissionService permissionService,
            IOrderModelFactory orderModelFactory,
            IWorkContext workContext,
            ICustomerActivityService customerActivityService,
            ILocalizationService localizationService)//IAddressAttributeParser addressAttributeParser, IAddressService addressService, ICustomerActivityService customerActivityService, IDateTimeHelper dateTimeHelper, IDownloadService downloadService, IEncryptionService encryptionService, IExportManager exportManager, IGiftCardService giftCardService, ILocalizationService localizationService, IOrderModelFactory orderModelFactory, IOrderProcessingService orderProcessingService, IOrderService orderService, IPaymentService paymentService, IPdfService pdfService, IPermissionService permissionService, IPriceCalculationService priceCalculationService, IProductAttributeFormatter productAttributeFormatter, IProductAttributeParser productAttributeParser, IProductAttributeService productAttributeService, IProductService productService, IShipmentService shipmentService, IShippingService shippingService, IShoppingCartService shoppingCartService, IWorkContext workContext, IWorkflowMessageService workflowMessageService, OrderSettings orderSettings) : base(addressAttributeParser, addressService, customerActivityService, dateTimeHelper, downloadService, encryptionService, exportManager, giftCardService, localizationService, orderModelFactory, orderProcessingService, orderService, paymentService, pdfService, permissionService, priceCalculationService, productAttributeFormatter, productAttributeParser, productAttributeService, productService, shipmentService, shippingService, shoppingCartService, workContext, workflowMessageService, orderSettings)
        {
            _orderService = orderService;
            _permissionService = permissionService;
            _orderModelFactory = orderModelFactory;
            _workContext = workContext;
            _customerActivityService = customerActivityService;
            _localizationService = localizationService;
            // hardcoded for now based on paypals 115% change allowed
            _allowedTotalChange = 1.15m;
        }

        /// <summary>
        /// Overrides default controller action, allowing vendors to save order totals(with caveats).
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Edit")]
        [FormValueRequired("btnSaveOrderTotals")]
        public IActionResult EditOrderTotals(int id, OrderModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            //try to get an order with the specified id
            var order = _orderService.GetOrderById(id);
            if (order == null)
                return RedirectToAction("List");
            
            // vendors can only edit shipping and it is subject to validation.
            if (_workContext.CurrentVendor != null
                && OrderShippingChangeValidation(order.OrderTotal,order.OrderShippingExclTax,model.OrderShippingExclTaxValue))
            {
                order.OrderShippingExclTax = model.OrderShippingExclTaxValue;
            }
            else
            {
                order.OrderSubtotalInclTax = model.OrderSubtotalInclTaxValue;
                order.OrderSubtotalExclTax = model.OrderSubtotalExclTaxValue;
                order.OrderSubTotalDiscountInclTax = model.OrderSubTotalDiscountInclTaxValue;
                order.OrderSubTotalDiscountExclTax = model.OrderSubTotalDiscountExclTaxValue;
                order.OrderShippingInclTax = model.OrderShippingInclTaxValue;
                order.OrderShippingExclTax = model.OrderShippingExclTaxValue;
                order.PaymentMethodAdditionalFeeInclTax = model.PaymentMethodAdditionalFeeInclTaxValue;
                order.PaymentMethodAdditionalFeeExclTax = model.PaymentMethodAdditionalFeeExclTaxValue;
                order.TaxRates = model.TaxRatesValue;
                order.OrderTax = model.TaxValue;
                order.OrderDiscount = model.OrderTotalDiscountValue;
                order.OrderTotal = model.OrderTotalValue;
            }
            
            _orderService.UpdateOrder(order);

            //add a note
            order.OrderNotes.Add(new OrderNote
            {
                Note = "Order totals have been edited",
                DisplayToCustomer = false,
                CreatedOnUtc = DateTime.UtcNow
            });
            _orderService.UpdateOrder(order);
            LogEditOrder(order.Id);

            //prepare model
            model = _orderModelFactory.PrepareOrderModel(model, order);

            return View(model);
        }

        protected virtual void LogEditOrder(int orderId)
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
