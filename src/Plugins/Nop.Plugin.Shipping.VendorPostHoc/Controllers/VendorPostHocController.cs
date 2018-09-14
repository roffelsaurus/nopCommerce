using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Directory;
using Nop.Plugin.Shipping.VendorPostHoc.Models;
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
    public class VendorPostHocController : BasePluginController
    {
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly VendorPostHocSettings _vendorPostHocSettings;

        public VendorPostHocController(
            ILocalizationService localizationService,
            IPermissionService permissionService,
            ISettingService settingService,
            VendorPostHocSettings vendorPostHocSettings)
        {
            _localizationService = localizationService;
            _permissionService = permissionService;
            _settingService = settingService;
            _vendorPostHocSettings = vendorPostHocSettings;
        }

        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShippingSettings))
                return AccessDeniedView();

            var model = new ConfigurationModel
            {
                ShippingCost = _vendorPostHocSettings.ShippingCost
            };
            
            return View("~/Plugins/Shipping.VendorPostHoc/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AdminAntiForgery]
        public IActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShippingSettings))
                return Content("Access denied");


            //save settings
            _vendorPostHocSettings.ShippingCost = model.ShippingCost;

            _settingService.SaveSetting(_vendorPostHocSettings);
            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));
            return Configure();
        }
    }
}