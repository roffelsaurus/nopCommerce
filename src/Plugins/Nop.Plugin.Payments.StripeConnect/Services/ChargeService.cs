using Newtonsoft.Json;
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
        private readonly StripeTokenService _stripetokenservice;

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
            _stripetokenservice = new StripeTokenService(_stripeConnectPaymentSettings.SecretKey);
        }


        public ProcessPaymentResult Charge(string token, decimal orderTotal, decimal orderSubTotal, int sellerCustomerId)
        {
            var stripecustomer = _customerEntityService.GetOrCreate(sellerCustomerId);
            var StripeBasedTotal = Convert.ToInt32(NopAmountToStripeAmount(orderTotal));
            var stripebasedSubTotal = NopAmountToStripeAmount(orderSubTotal);
            

            // fee is 20% of total without shipping
            var appFee = Convert.ToInt32(Math.Floor(stripebasedSubTotal * 0.2m)); // todo configurable appfee percent
            var chargeAmount = StripeBasedTotal - appFee;

            var chargeOptions = new StripeChargeCreateOptions()
            {
                Amount = chargeAmount,
                SourceTokenOrExistingSourceId = token,
                ApplicationFee = appFee,
                Currency = "USD" // TODO currency
            };
            _logger.Information("Stripe charge with Token: " + token + ", Amount: " + chargeAmount + ", AppFee: " + appFee);


            var charge = _stripeChargeService.Create(chargeOptions, new StripeRequestOptions()
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

        private decimal NopAmountToStripeAmount(decimal amount)
        {
            return Math.Floor(amount * 100m); // TODO fix currency to use subcurrency, not 100m
        }

    }
}
