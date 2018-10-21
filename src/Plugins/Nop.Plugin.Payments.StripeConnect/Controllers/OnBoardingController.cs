using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Plugin.Payments.StripeConnect.Models;
using Nop.Plugin.Payments.StripeConnect.Services;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Web.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Security;

namespace Nop.Plugin.Payments.StripeConnect.Controllers
{
    public partial class OnBoardingController : BasePublicController
    {
        private readonly IWorkContext _workContext;
        private readonly IOnBoardingService _onBoardingService;
        private readonly ILogger _logger;
        private readonly ILocalizationService _localizationService;

        public OnBoardingController(IWorkContext workContext,
            IOnBoardingService onBoardingService,
            ILogger logger,
            ILocalizationService localizationService

            )
        {
            _workContext = workContext;
            _onBoardingService = onBoardingService;
            _logger = logger;
            _localizationService = localizationService;
        }

        [HttpsRequirement(SslRequirement.Yes)]
        public IActionResult Index(bool consent = false)
        {
            if (!_workContext.CurrentCustomer.IsRegistered())
                return Challenge();
            var model = new OnBoardingModel();

            model.OnBoardingUrl = _onBoardingService.NewOnboarding();

            return View("~/Plugins/Payments.StripeConnect/Views/OnBoarding.cshtml", model);
        }

        [HttpsRequirement(SslRequirement.Yes)]
        public IActionResult Redirect([FromQuery]string code, [FromQuery]string scope, [FromQuery]string state)
        {
            if (!_workContext.CurrentCustomer.IsRegistered())
                return Challenge();
            var model = new OnBoardingModel();

            if (!_onBoardingService.ValidCSRFToken(state))
            {
                ErrorNotification(_localizationService.GetResource("Plugin.Payments.StripeConnect.OnBoarding.InvalidToken",
                _workContext.WorkingLanguage.Id,
                defaultValue: "Invalid token"));
                return View("~/Plugins/Payments.StripeConnect/Views/OnBoarding.cshtml", model);
            }

            // todo use code to get final code and persist it


            return View("~/Plugins/Payments.StripeConnect/Views/OnBoarding.cshtml", model);
        }



    }
}