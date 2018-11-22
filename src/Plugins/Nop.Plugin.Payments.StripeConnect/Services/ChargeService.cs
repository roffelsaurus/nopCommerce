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
    using Nop.Plugin.Payments.StripeConnect.Domain;
    using static StripeUtils;
    public class ChargeService : IChargeService
    {
        private readonly IWorkContext _workContext;
        private readonly ILogger _logger;
        private readonly StripeConnectPaymentSettings _stripeConnectPaymentSettings;
        private readonly IWebHelper _webHelper;
        private readonly HttpClient _httpClient;
        private readonly ICustomerEntityService _customerEntityService;
        private readonly IOrderChargeEntityService _orderChargeEntityService;
        private readonly StripeChargeService _stripeChargeService;
        private readonly StripeTokenService _stripetokenservice;

        public ChargeService(IWorkContext workContext,
            ILogger logger,
            StripeConnectPaymentSettings stripeConnectPaymentSettings,
            IWebHelper webHelper,

            HttpClient httpClient,
            ICustomerEntityService customerEntityService,
            IOrderChargeEntityService orderChargeEntityService)
        {
            _workContext = workContext;
            _logger = logger;
            _stripeConnectPaymentSettings = stripeConnectPaymentSettings;
            _webHelper = webHelper;
            _httpClient = httpClient;
            _customerEntityService = customerEntityService;
            _orderChargeEntityService = orderChargeEntityService;
            _stripeChargeService = new StripeChargeService(_stripeConnectPaymentSettings.SecretKey);
            _stripetokenservice = new StripeTokenService(_stripeConnectPaymentSettings.SecretKey);
        }
        
        public ProcessPaymentResult Charge(string token,
            decimal orderSubTotal, 
            int sellerCustomerId,
            ProcessPaymentRequest processPaymentRequest)
        {
            var stripeSeller = _customerEntityService.GetOrCreate(sellerCustomerId);
            var StripeBasedTotal = NopDecimalToStripeInt(processPaymentRequest.OrderTotal);
            var stripebasedSubTotal = NopDecimalToStripeDecimal(orderSubTotal);
            
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
                IdempotencyKey = processPaymentRequest.OrderGuid.ToString("N"),
                StripeConnectAccountId = stripeSeller.StripeUserId
            });
            var result = new ProcessPaymentResult();
            if (charge.Paid)
            {
                result.NewPaymentStatus = Core.Domain.Payments.PaymentStatus.Paid;
                var stripeOrderCharge = new StripeOrderCharge();
                stripeOrderCharge.CustomerId = processPaymentRequest.CustomerId;
                stripeOrderCharge.SellerCustomerId = sellerCustomerId;
                stripeOrderCharge.OrderGuid = processPaymentRequest.OrderGuid;
                stripeOrderCharge.StripeChargeId = charge.Id;
                _orderChargeEntityService.Create(stripeOrderCharge);
            }
            else
            {
                result.AddError("Error " + charge.FailureCode + " " + charge.FailureMessage);
            }
            return result;
        }
    }
}
