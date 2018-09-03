using Nop.Core.Domain.Orders;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Shipping.VendorPostHoc.Models
{
    public class AdjustOrderShippingModel
    {
        public int Id { get; set; }
        [NopResourceDisplayName("Admin.Orders.Fields.CustomOrderNumber")]
        public string CustomOrderNumber { get; set; }

        [NopResourceDisplayName("Admin.Orders.Fields.Edit.OrderShipping")]
        public decimal OrderShippingExclTaxValue { get; set; }

        public AdjustOrderShippingModel(Order order)
        {
            Id = order.Id;
            CustomOrderNumber = order.CustomOrderNumber;
            OrderShippingExclTaxValue = order.OrderShippingExclTax;
        }
    }
}
