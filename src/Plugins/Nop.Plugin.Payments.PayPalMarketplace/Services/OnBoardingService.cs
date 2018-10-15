using BraintreeHttp;
using Nop.Core;
using Nop.Services.Logging;
using PayPal.Core;
using PayPal.v1.PartnerReferrals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Nop.Plugin.Payments.PayPalMarketplace.Services
{

    public class OnBoardingService : IOnBoardingService
    {
        private readonly IWorkContext _workContext;
        private readonly PayPalHttpClient _payPalHttpClient;
        private readonly ILogger _logger;
        private readonly PayPalMarketPlacePaymentSettings _payPalMarketPlacePaymentSettings;

        public OnBoardingService(IWorkContext workContext,
            PayPalHttpClient payPalHttpClient,
            ILogger logger,
            PayPalMarketPlacePaymentSettings payPalMarketPlacePaymentSettings)
        {
            _workContext = workContext;
            _payPalHttpClient = payPalHttpClient;
            _logger = logger;
            _payPalMarketPlacePaymentSettings = payPalMarketPlacePaymentSettings;
        }
        
        public string GetActionUrl()
        {
            var referral = new PartnerReferral()
            {
                CustomerData = new PayPal.v1.PartnerReferrals.User.User()
                {
                    PartnerSpecificIdentifiers = new PayPal.v1.PartnerReferrals.User.PartnerSpecificIdentifier[]
                    {
                        new PayPal.v1.PartnerReferrals.User.PartnerSpecificIdentifier()
                        {
                            Type = PayPal.v1.PartnerReferrals.User.PartnerSpecificIdentifierType.TRACKING_ID,
                            Value = _workContext.CurrentVendor.Id.ToString() // good enough ID?
                        }
                    }
                },
                RequestedCapabilities = new PayPal.v1.PartnerReferrals.Capabilities.Capability[]
                {
                    new PayPal.v1.PartnerReferrals.Capabilities.Capability()
                    {
                        CapabilityType = PayPal.v1.PartnerReferrals.Capabilities.CapabilityType.API_INTEGRATION,
                        ApiIntegrationPreference =
                        {
                            PartnerId = _payPalMarketPlacePaymentSettings.PartnerId, // Our paypal id
                            RestApiIntegration = new PayPal.v1.PartnerReferrals.Capabilities.RestApiIntegration()
                            {
                                IntegrationMethod = PayPal.v1.PartnerReferrals.Capabilities.IntegrationMethod.PAYPAL,
                                IntegrationType = PayPal.v1.PartnerReferrals.Capabilities.IntegrationType.THIRD_PARTY
                            },
                            RestThirdPartyDetails = new PayPal.v1.PartnerReferrals.Capabilities.RestThirdPartyDetails()
                            {
                                PartnerClientId = _payPalMarketPlacePaymentSettings.ClientId,
                                RestEndpointFeatures = new PayPal.v1.PartnerReferrals.Capabilities.RestEndpointFeature[]
                                {
                                    PayPal.v1.PartnerReferrals.Capabilities.RestEndpointFeature.PAYMENT,
                                    PayPal.v1.PartnerReferrals.Capabilities.RestEndpointFeature.REFUND,
                                    PayPal.v1.PartnerReferrals.Capabilities.RestEndpointFeature.PARTNER_FEE,
                                    PayPal.v1.PartnerReferrals.Capabilities.RestEndpointFeature.DELAY_FUNDS_DISBURSEMENT,
                                    PayPal.v1.PartnerReferrals.Capabilities.RestEndpointFeature.READ_SELLER_DISPUTE,
                                    PayPal.v1.PartnerReferrals.Capabilities.RestEndpointFeature.UPDATE_SELLER_DISPUTE
                                }
                            }
                        }
                    }
                },
                WebExperiencePreference = new WebExperiencePreference()
                {
                    PartnerLogoUrl = "", // logo ? 
                    UseMiniBrowser = true
                },
                CollectedConsents = new LegalConsent[]
                {
                    new LegalConsent()
                    {
                        Type = LegalConsentType.SHARE_DATA_CONSENT,
                        Granted = true
                    }
                },
                Products = new ProductName[]
                {
                    ProductName.EXPRESS_CHECKOUT
                }
            };

            var request = new PartnerReferralCreateRequest().RequestBody(referral);
            PartnerReferralCreateResponse response = null;
            HttpStatusCode responsecode;
            try
            {
                var task = _payPalHttpClient.Execute(request);
                task.Wait(5000);
                responsecode = task.Result.StatusCode;
                response = task.Result.Result<PartnerReferralCreateResponse>();
                return response?.Links?.SingleOrDefault(i => i.Rel.Equals("action_url")).Href;
            }
            catch (HttpException httpException)
            {
                var debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();
                _logger.Error($"payPalHttpClient.Execute failed (statuscode: { httpException.StatusCode}, pp-debugid: {debugId})", httpException.InnerException);
            }
            catch (Exception e)
            {
                _logger.Error($"payPalHttpClient.Execute failed for other reason {e.Message}", e.InnerException);
            }
            return null;
        }
    }
}
