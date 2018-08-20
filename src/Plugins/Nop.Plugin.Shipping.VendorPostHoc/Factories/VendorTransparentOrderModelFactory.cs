//using Microsoft.AspNetCore.Mvc.Infrastructure;
//using Microsoft.AspNetCore.Mvc.Routing;
//using Nop.Core;
//using Nop.Core.Domain.Common;
//using Nop.Core.Domain.Directory;
//using Nop.Core.Domain.Orders;
//using Nop.Core.Domain.Shipping;
//using Nop.Core.Domain.Tax;
//using Nop.Services.Affiliates;
//using Nop.Services.Catalog;
//using Nop.Services.Common;
//using Nop.Services.Directory;
//using Nop.Services.Discounts;
//using Nop.Services.Helpers;
//using Nop.Services.Localization;
//using Nop.Services.Media;
//using Nop.Services.Orders;
//using Nop.Services.Payments;
//using Nop.Services.Security;
//using Nop.Services.Shipping;
//using Nop.Services.Stores;
//using Nop.Services.Tax;
//using Nop.Services.Vendors;
//using Nop.Web.Areas.Admin.Factories;
//using Nop.Web.Areas.Admin.Models.Orders;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Nop.Plugin.Shipping.VendorPostHoc.Factories
//{
//    public class VendorTransparentOrderModelFactory : OrderModelFactory
//    {
//        public VendorTransparentOrderModelFactory(AddressSettings addressSettings, CurrencySettings currencySettings, IActionContextAccessor actionContextAccessor, IAddressAttributeFormatter addressAttributeFormatter, IAddressAttributeModelFactory addressAttributeModelFactory, IAffiliateService affiliateService, IBaseAdminModelFactory baseAdminModelFactory, ICountryService countryService, ICurrencyService currencyService, IDateTimeHelper dateTimeHelper, IDiscountService discountService, IDownloadService downloadService, IEncryptionService encryptionService, IGiftCardService giftCardService, ILocalizationService localizationService, IMeasureService measureService, IOrderProcessingService orderProcessingService, IOrderReportService orderReportService, IOrderService orderService, IPaymentService paymentService, IPictureService pictureService, IPriceCalculationService priceCalculationService, IPriceFormatter priceFormatter, IProductAttributeService productAttributeService, IProductService productService, IReturnRequestService returnRequestService, IShipmentService shipmentService, IShippingService shippingService, IStoreService storeService, ITaxService taxService, IUrlHelperFactory urlHelperFactory, IVendorService vendorService, IWorkContext workContext, MeasureSettings measureSettings, OrderSettings orderSettings, ShippingSettings shippingSettings, TaxSettings taxSettings) : base(addressSettings, currencySettings, actionContextAccessor, addressAttributeFormatter, addressAttributeModelFactory, affiliateService, baseAdminModelFactory, countryService, currencyService, dateTimeHelper, discountService, downloadService, encryptionService, giftCardService, localizationService, measureService, orderProcessingService, orderReportService, orderService, paymentService, pictureService, priceCalculationService, priceFormatter, productAttributeService, productService, returnRequestService, shipmentService, shippingService, storeService, taxService, urlHelperFactory, vendorService, workContext, measureSettings, orderSettings, shippingSettings, taxSettings)
//        {
//        }

//        public override OrderSearchModel PrepareOrderSearchModel(OrderSearchModel searchModel)
//        {
//            var model = base.PrepareOrderSearchModel(searchModel);
//            model.IsLoggedInAsVendor = false;
//            return model;
//        }

//        public override OrderModel PrepareOrderModel(OrderModel model, Order order, bool excludeProperties = false)
//        {
//            var viewmodel = base.PrepareOrderModel(model, order, excludeProperties);
//            viewmodel.IsLoggedInAsVendor = false;
//            return viewmodel;
//        }
//    }
//}
