using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Nop.Core;
using Nop.Core.Domain;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Forums;
using Nop.Core.Domain.Gdpr;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Tax;
using Nop.Plugin.Payments.PayPalMarketplace.Models;
using Nop.Services.Authentication;
using Nop.Services.Authentication.External;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Events;
using Nop.Services.ExportImport;
using Nop.Services.Gdpr;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Tax;
using Nop.Web.Controllers;
using Nop.Web.Extensions;
using Nop.Web.Factories;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Security;
using Nop.Web.Framework.Security.Captcha;
using Nop.Web.Framework.Validators;
using Nop.Web.Models.Customer;
using PayPal.Core;

namespace Nop.Plugin.Payments.PayPalMarketplace.Controllers
{
    public partial class OnBoardingController : BasePublicController
    {
        private readonly IWorkContext _workcontext;
        private readonly PayPalHttpClient _paypalhttpclient;

        public OnBoardingController(IWorkContext workContext,
            PayPalHttpClient payPalHttpClient)
        {
            _workcontext = workContext;
            _paypalhttpclient = payPalHttpClient;
        }
        [HttpsRequirement(SslRequirement.Yes)]
        public virtual IActionResult Index()
        {
            if (!_workcontext.CurrentCustomer.IsRegistered())
                return Challenge();
            var model = new OnBoardingModel();
            return View("~/Plugins/Payments.PayPalMarketplace/Views/OnBoarding.cshtml", model);
        }

        [HttpsRequirement(SslRequirement.Yes)]
        public virtual IActionResult OnBoard(OnBoardingModel model)
        {
            if (!_workcontext.CurrentCustomer.IsRegistered())
                return Challenge();

           //  _paypalhttpclient.Execute()


            return View("~/Plugins/Payments.PayPalMarketplace/Views/OnBoarding.cshtml", model);
        }

    }
}