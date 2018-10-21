using Nop.Core;
using Nop.Services.Logging;
using System;
using System.Linq;
using System.Net;

namespace Nop.Plugin.Payments.StripeConnect.Services
{

    public class OnBoardingService : IOnBoardingService
    {
        private readonly IWorkContext _workContext;
        private readonly ILogger _logger;
        private readonly StripeConnectPaymentSettings _stripeConnectPaymentSettings;

        public OnBoardingService(IWorkContext workContext,
            ILogger logger,
            StripeConnectPaymentSettings stripeConnectPaymentSettings)
        {
            _workContext = workContext;
            _logger = logger;
            _stripeConnectPaymentSettings = stripeConnectPaymentSettings;
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
