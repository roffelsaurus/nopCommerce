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
using System.Security.Cryptography;

namespace Nop.Plugin.Payments.StripeConnect.Services
{

    public class OnBoardingService : IOnBoardingService
    {
        private readonly IWorkContext _workContext;
        private readonly ILogger _logger;
        private readonly StripeConnectPaymentSettings _stripeConnectPaymentSettings;
        private readonly IWebHelper _webHelper;
        private readonly IUrlHelper _urlHelper;
        private readonly ConcurrentDictionary<string, int> _onboardingCustomers; // key csrf token, customer id val

        public OnBoardingService(IWorkContext workContext,
            ILogger logger,
            StripeConnectPaymentSettings stripeConnectPaymentSettings,
            IWebHelper webHelper,
            IUrlHelper urlHelper)
        {
            _workContext = workContext;
            _logger = logger;
            _stripeConnectPaymentSettings = stripeConnectPaymentSettings;
            _webHelper = webHelper;
            _urlHelper = urlHelper;
            _onboardingCustomers = new ConcurrentDictionary<string, int>();
        }

        public bool ValidCSRFToken(string token)
        {
            if (!_onboardingCustomers.ContainsKey(token))
                return false;

            if (!_onboardingCustomers[token].Equals(_workContext.CurrentCustomer.Id))
                return false;

            return true;
        }

        public string NewOnboarding()
        {
            var csrftoken = GenerateString(16);
            _onboardingCustomers.AddOrUpdate(csrftoken, _workContext.CurrentCustomer.Id, (_, id) => id);


            var parameters = new Dictionary<string, string>()
            {
                ["client_id"] = _stripeConnectPaymentSettings.ClientId,
                ["response_type"] = "code",
                ["redirect_uri"] = _urlHelper.RouteUrl(RouteProvider.ONBOARDING_REDIRECT), // $"{_webHelper.GetStoreLocation()}Admin/PaymentStripeConnect/Configure";
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

        //public PartnerReferral CreateNewReferral()
        //{
        //    var referral = new PartnerReferral()
        //    {
        //        CustomerData = new User()
        //        {
        //            PartnerSpecificIdentifiers = new PartnerSpecificIdentifier[]
        //            {
        //                new PartnerSpecificIdentifier()
        //                {
        //                    PartnerSpecificIdentifierType = PartnerSpecificIdentifierType.TRACKING_ID,
        //                    Value = _workContext.CurrentCustomer.Id.ToString()
        //                }
        //            }
        //        },
        //        RequestedCapabilities = new Capability[]
        //        {
        //            new Capability()
        //            {
        //                CapabilityType = CapabilityType.API_INTEGRATION,
        //                ApiIntegrationPreference = new ApiIntegrationPreference()
        //                {
        //                    PartnerId = _payPalMarketPlacePaymentSettings.PartnerId,
        //                    RestApiIntegration = new RestApiIntegration()
        //                    {
        //                        IntegrationMethod = IntegrationMethod.PAYPAL,
        //                        IntegrationType = IntegrationType.THIRD_PARTY
        //                    },
        //                    RestThirdPartyDetails = new RestThirdPartyDetails()
        //                    {
        //                        PartnerClientId = _payPalMarketPlacePaymentSettings.ClientId,
        //                        RestEndpointFeatures = new string[]
        //                        {
        //                            RestEndpointFeature.PAYMENT,
        //                            RestEndpointFeature.REFUND,
        //                            RestEndpointFeature.PARTNER_FEE,
        //                            RestEndpointFeature.DELAY_FUNDS_DISBURSEMENT,
        //                            RestEndpointFeature.READ_SELLER_DISPUTE,
        //                            RestEndpointFeature.UPDATE_SELLER_DISPUTE
        //                        }
        //                    }
        //                }
        //            }
        //        },
        //        WebExperiencePreference = new WebExperiencePreference()
        //        {
        //            PartnerLogoUrl = "",
        //            UseMiniBrowser = true
        //        },
        //        CollectedConsents = new LegalConsent[]
        //        {
        //            new LegalConsent()
        //            {
        //                Type = LegalConsentType.SHARE_DATA_CONSENT,
        //                Granted = true
        //            }
        //        },
        //        Products = new string[]
        //        {
        //            ProductNames.EXPRESS_CHECKOUT
        //        }
        //    };
        //    return referral;
        //}

        //public string GetActionUrl(PartnerReferral referral)
        //{
        //    try
        //    {
        //        var request = new PartnerReferralCreateRequest().RequestBody(referral);
        //        PartnerReferralCreateResponse response = null;
        //        HttpStatusCode responsecode;
        //        var task = _payPalHttpClient.Execute(request);
        //        task.Wait(5000);
        //        responsecode = task.Result.StatusCode;
        //        response = task.Result.Result<PartnerReferralCreateResponse>();
        //        return response?.Links?.SingleOrDefault(i => i.Rel.Equals("action_url"))?.Href;
        //    }
        //    catch (HttpException httpException)
        //    {
        //        var debugId = httpException?.Headers?.GetValues("PayPal-Debug-Id")?.FirstOrDefault();
        //        _logger.Error($"payPalHttpClient.Execute failed (statuscode: { httpException?.StatusCode}, pp-debugid: {debugId})");
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.Error($"payPalHttpClient.Execute failed for other reason {e?.Message}");
        //    }
        //    return null;
        //}
    }
}
