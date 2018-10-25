using System;
using System.Collections.Generic;
using System.Net.Http;
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
        private readonly ICustomerEntityService _customerEntityService;

        public OnBoardingController(IWorkContext workContext,
            IOnBoardingService onBoardingService,
            ILogger logger,
            ILocalizationService localizationService,
            ICustomerEntityService customerEntityService

            )
        {
            _workContext = workContext;
            _onBoardingService = onBoardingService;
            _logger = logger;
            _localizationService = localizationService;
            _customerEntityService = customerEntityService;
        }

        [HttpsRequirement(SslRequirement.Yes)]
        public IActionResult Index()
        {
            if (!_workContext.CurrentCustomer.IsRegistered())
                return Challenge();
            var domain = _customerEntityService.GetOrCreate(_workContext.CurrentCustomer.Id);

            var model = new OnBoardingModel();
            if (!string.IsNullOrEmpty(domain.AccessToken))
            {
                model.AlreadyOnboard = true;
            }
            else
                model.OnBoardingUrl = _onBoardingService.NewOnboarding();

            return View("~/Plugins/Payments.StripeConnect/Views/OnBoarding.cshtml", model);
        }

        [HttpsRequirement(SslRequirement.Yes)]
        public IActionResult Redirect([FromQuery]string code, [FromQuery]string scope, [FromQuery]string state)
        {
            if (!_workContext.CurrentCustomer.IsRegistered())
                return Challenge();
            var model = new OnBoardingModel();
            var customerId = _onBoardingService.ConsumeCustomerMappingForCSRFToken(state);
            if (customerId == null || !customerId.HasValue)
            {
                ErrorNotification(_localizationService.GetResource("Plugin.Payments.StripeConnect.OnBoarding.InvalidToken",
                _workContext.WorkingLanguage.Id,
                defaultValue: "Invalid token"));
                return View("~/Plugins/Payments.StripeConnect/Views/OnBoarding.cshtml", model);
            }

            var success = _onBoardingService.GetNewAccessToken(code, customerId.Value);
            if (success)
            {
                SuccessNotification(_localizationService.GetResource("Plugin.Payments.StripeConnect.OnBoarding.Success",
                _workContext.WorkingLanguage.Id,
                defaultValue: "Onboarding successful!"));
                model.AlreadyOnboard = true;
            } else
            {
                ErrorNotification(_localizationService.GetResource("Plugin.Payments.StripeConnect.OnBoarding.Fail",
                _workContext.WorkingLanguage.Id,
                defaultValue: "Onboarding failed."));
            }

            return View("~/Plugins/Payments.StripeConnect/Views/OnBoarding.cshtml", model);
        }



    }
}