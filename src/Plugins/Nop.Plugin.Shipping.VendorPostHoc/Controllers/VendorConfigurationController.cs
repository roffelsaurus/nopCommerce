using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Directory;
using Nop.Data;
using Nop.Plugin.Shipping.VendorPostHoc.Domain;
using Nop.Plugin.Shipping.VendorPostHoc.Models;
using Nop.Plugin.Shipping.VendorPostHoc.Services;
using Nop.Services.Configuration;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Services.Shipping;
using Nop.Services.Stores;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Shipping.VendorPostHoc.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public class VendorConfigurationController : BasePluginController
    {
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly VendorPostHocSettings _vendorPostHocSettings;
        private readonly IWorkContext _workcontext;
        private readonly IVendorConfigurationService _vendorconfigurationservice;

        public VendorConfigurationController(
            ILocalizationService localizationService,
            IPermissionService permissionService,
            ISettingService settingService,
            VendorPostHocSettings vendorPostHocSettings,
            IWorkContext workContext,
            IVendorConfigurationService vendorConfigurationService)
        {
            _localizationService = localizationService;
            _permissionService = permissionService;
            _settingService = settingService;
            _vendorPostHocSettings = vendorPostHocSettings;
            _workcontext = workContext;
            _vendorconfigurationservice = vendorConfigurationService;
        }

        public IActionResult Edit()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.AccessAdminPanel))
                return AccessDeniedView();

            var vendor = _workcontext.CurrentVendor;
            if (vendor == null)
            {
                return AccessDeniedView();
            }

            var domainmodel = _vendorconfigurationservice.GetForVendor(vendor.Id);


            var model = new VendorConfigurationModel
            {
                ShippingCost = domainmodel.ShippingCost
            };
            
            return View("~/Plugins/Shipping.VendorPostHoc/Views/VendorConfiguration.cshtml", model);
        }

        [HttpPost]
        [AdminAntiForgery]
        public IActionResult Edit(VendorConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.AccessAdminPanel))
                return Content("Access denied");

            var domainmodel = _vendorconfigurationservice.GetForVendor(_workcontext.CurrentVendor.Id);
            domainmodel.ShippingCost = model.ShippingCost;
            _vendorconfigurationservice.Update(domainmodel);

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));
            return Edit();
        }
    }
}