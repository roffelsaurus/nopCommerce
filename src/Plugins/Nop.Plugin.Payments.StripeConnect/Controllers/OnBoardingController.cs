using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Plugin.Payments.StripeConnect.Models;
using Nop.Plugin.Payments.StripeConnect.Services;
using Nop.Services.Logging;
using Nop.Web.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Security;

namespace Nop.Plugin.Payments.StripeConnect.Controllers
{
    public partial class OnBoardingController : BasePublicController
    {
        private readonly IWorkContext _workcontext;
        private readonly IOnBoardingService _onBoardingService;
        private readonly ILogger _logger;

        public OnBoardingController(IWorkContext workContext,
            IOnBoardingService onBoardingService,
            ILogger logger

            )
        {
            _workcontext = workContext;
            _onBoardingService = onBoardingService;
            _logger = logger;
        }

        [HttpsRequirement(SslRequirement.Yes)]
        public IActionResult Index(bool consent = false)
        {
            if (!_workcontext.CurrentCustomer.IsRegistered())
                return Challenge();
            var model = new OnBoardingModel();
            if (consent)
            {

            }
            return View("~/Plugins/Payments.StripeConnect/Views/OnBoarding.cshtml", model);
        }
    }
}