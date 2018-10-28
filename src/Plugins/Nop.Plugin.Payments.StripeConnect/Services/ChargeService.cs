using Nop.Core;
using Nop.Services.Logging;
using Nop.Services.Payments;
using Stripe;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Nop.Plugin.Payments.StripeConnect.Services
{
    public class ChargeService : IChargeService
    {
        private readonly IWorkContext _workContext;
        private readonly ILogger _logger;
        private readonly StripeConnectPaymentSettings _stripeConnectPaymentSettings;
        private readonly IWebHelper _webHelper;
        private readonly HttpClient _httpClient;
        private readonly ICustomerEntityService _customerEntityService;
        private readonly StripeChargeService _stripeChargeService;

        public ChargeService(IWorkContext workContext,
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
            _stripeChargeService = new StripeChargeService(_stripeConnectPaymentSettings.SecretKey);
        }


        public ProcessPaymentResult Charge(string token,decimal amount, int sellerCustomerId)
        {
            var stripecustomer = _customerEntityService.GetOrCreate(sellerCustomerId);
            var stripeAmount = NopAmountToStripeAmount(amount);
            var appFee = (int)(stripeAmount * 0.2);
            var chargeStripeAmount = stripeAmount - appFee;
            var charge = _stripeChargeService.Create(new StripeChargeCreateOptions()
            {
                Amount = chargeStripeAmount,
                SourceTokenOrExistingSourceId = token,
                ApplicationFee = appFee,
                Currency = "USD" // TODO currency
            }, new StripeRequestOptions()
            {
                // IdempotencyKey = GetIdempotencyKey(orderId) TODO include this?
                StripeConnectAccountId = stripecustomer.StripeUserId
            });
            return new ProcessPaymentResult()
            {
                NewPaymentStatus = charge.Paid ?
                Core.Domain.Payments.PaymentStatus.Paid : Core.Domain.Payments.PaymentStatus.Pending
            };
        }

        private int? NopAmountToStripeAmount(decimal amount)
        {
            return (int)(amount * 100); // TODO fix currency
        }
    }
}
