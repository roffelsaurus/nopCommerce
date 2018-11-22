using Nop.Core;
using Nop.Services.Logging;
using Nop.Services.Payments;
using Stripe;
using System.Net.Http;

namespace Nop.Plugin.Payments.StripeConnect.Services
{
    using static StripeUtils;
    public class RefundService : IRefundService
    {
        private readonly IWorkContext _workContext;
        private readonly ILogger _logger;
        private readonly StripeConnectPaymentSettings _stripeConnectPaymentSettings;
        private readonly IWebHelper _webHelper;
        private readonly HttpClient _httpClient;
        private readonly ICustomerEntityService _customerEntityService;
        private readonly IChargeService _chargeService;
        private readonly IOrderChargeEntityService _chargeEntityService;
        private readonly StripeRefundService _stripeRefundService;

        public RefundService
            (IWorkContext workContext,
            ILogger logger,
            StripeConnectPaymentSettings stripeConnectPaymentSettings,
            IWebHelper webHelper,
            HttpClient httpClient,
            ICustomerEntityService customerEntityService,
            IChargeService chargeService,
            IOrderChargeEntityService chargeEntityService)
        {
            _workContext = workContext;
            _logger = logger;
            _stripeConnectPaymentSettings = stripeConnectPaymentSettings;
            _webHelper = webHelper;
            _httpClient = httpClient;
            _customerEntityService = customerEntityService;
            _chargeService = chargeService;
            _chargeEntityService = chargeEntityService;
            _stripeRefundService = new StripeRefundService(_stripeConnectPaymentSettings.SecretKey);
        }

        public RefundPaymentResult Refund(RefundPaymentRequest refundPaymentRequest)
        {
            var stripeCharge = _chargeEntityService.Get(refundPaymentRequest.Order.OrderGuid);

            var customerDomainModel = _customerEntityService.GetOrCreate(stripeCharge.SellerCustomerId);
            var requestOptions = new StripeRequestOptions();
            requestOptions.StripeConnectAccountId = customerDomainModel.StripeUserId;
            
            var refund = _stripeRefundService.Create(stripeCharge.StripeChargeId, requestOptions: requestOptions);
            var result = new RefundPaymentResult();
            if (refund.Status == "succeeded")
            {
                result.NewPaymentStatus = Core.Domain.Payments.PaymentStatus.Refunded;
            }
            else
            {
                result.AddError("Error " + refund.FailureReason);
            }
            return result;
        }
    }
}
