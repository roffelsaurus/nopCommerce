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

        public VendorPostHocController(
            ILocalizationService localizationService,
            IPermissionService permissionService,
            ISettingService settingService)
        {
            _localizationService = localizationService;
            _permissionService = permissionService;
            _settingService = settingService;
        }

        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShippingSettings))
                return AccessDeniedView();

            var model = new ConfigurationModel
            {
            };
            
            return View("~/Plugins/Shipping.VendorPostHoc/Views/Configure.cshtml", model);
        }

        //[HttpPost]
        //[AdminAntiForgery]
        //public IActionResult Configure(ConfigurationModel model)
        //{
        //    if (!_permissionService.Authorize(StandardPermissionProvider.ManageShippingSettings))
        //        return Content("Access denied");

        //    //save settings
        //   // _fixedByWeightByTotalSettings.LimitMethodsToCreated = model.LimitMethodsToCreated;
        //    //_settingService.SaveSetting(_fixedByWeightByTotalSettings);

        //    return Json(new { Result = true });
        //}
    }
}