using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Plugin.Payments.StripeConnect.Infrastructure;
using Nop.Services.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Stripe;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Payments.StripeConnect.Services
{

    public class OnBoardingService : IOnBoardingService
    {
        private readonly IWorkContext _workContext;
        private readonly ILogger _logger;
        private readonly StripeConnectPaymentSettings _stripeConnectPaymentSettings;
        private readonly IWebHelper _webHelper;
        private readonly HttpClient _httpClient;
        private readonly ICustomerEntityService _customerEntityService;
        private readonly ConcurrentDictionary<string, int> _onboardingCustomers; // key csrf token, customer id val
        private readonly StripeOAuthTokenService _stripeOAuthTokenService;

        public OnBoardingService(IWorkContext workContext,
            ILogger logger,
            StripeConnectPaymentSettings stripeConnectPaymentSettings,
            IWebHelper webHelper,

            HttpClient httpClient,
            ICustomerEntityService customerEntityService)
        {
            _workContext = workContext;
            _logger = logger;
            _stripeConnectPaymentSettings = stripeConnectPaymentSettings;
            _webHelper = webHelper;
            _httpClient = httpClient;
            _customerEntityService = customerEntityService;
            _onboardingCustomers = new ConcurrentDictionary<string, int>();
            _stripeOAuthTokenService = new StripeOAuthTokenService(_stripeConnectPaymentSettings.SecretKey);
        }

        public int? ConsumeCustomerMappingForCSRFToken(string token)
        {
            if (!_onboardingCustomers.ContainsKey(token))
                return null;

            if (!_onboardingCustomers[token].Equals(_workContext.CurrentCustomer.Id))
                return null;

            if (_onboardingCustomers.TryRemove(token, out var retval))
                return retval;
            else
                return null;
        }

        public string NewOnboarding()
        {
            var csrftoken = GenerateString(16);
            _onboardingCustomers.AddOrUpdate(csrftoken, _workContext.CurrentCustomer.Id, (_, id) => id);


            var parameters = new Dictionary<string, string>()
            {
                ["client_id"] = _stripeConnectPaymentSettings.ClientId,
                ["response_type"] = "code",
                ["redirect_uri"] = $"{_webHelper.GetStoreLocation()}{RouteProvider.ONBOARDING_REDIRECT_ROUTE}",
                ["scope"] = "read_write",
                ["state"] = csrftoken,
                ["stripe_landing"] = "register"
            };
            return QueryHelpers.AddQueryString("https://connect.stripe.com/oauth/authorize", parameters);
        }
        
        private static string GenerateString(int length)
        {
            const string AllowableCharacters = "abcdefghijklmnopqrstuvwxyz0123456789";
            var bytes = new byte[length];

            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(bytes);
            }

            return new string(bytes.Select(x => AllowableCharacters[x % AllowableCharacters.Length]).ToArray());
        }

        public bool GetNewAccessToken(string authCode, int customerId)
        {
            var oAuthToken = _stripeOAuthTokenService.Create(new StripeOAuthTokenCreateOptions()
            {
                GrantType = "authorization_code",
                Code = authCode
            });
            if (!string.IsNullOrEmpty(oAuthToken.Error))
            {
                _logger.Error($"GetNewAccessToken fail: error: {oAuthToken.Error} desc: {oAuthToken.ErrorDescription}");
                return false;
            }
            var domain = _customerEntityService.GetOrCreate(customerId);
            domain.AccessToken = oAuthToken.AccessToken;
            domain.RefreshToken = oAuthToken.RefreshToken;
            domain.StripePublishableKey = oAuthToken.StripePublishableKey;
            domain.StripeUserId = oAuthToken.StripeUserId;
            _customerEntityService.Update(domain);
            return true;
        }


    }
}
