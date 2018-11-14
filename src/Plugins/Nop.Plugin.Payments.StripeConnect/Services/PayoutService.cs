using Nop.Core;
using Nop.Services.Logging;
using Stripe;
using System.Net.Http;

namespace Nop.Plugin.Payments.StripeConnect.Services
{
    public class PayoutService
    {
        private readonly IWorkContext _workContext;
        private readonly ILogger _logger;
        private readonly StripeConnectPaymentSettings _stripeConnectPaymentSettings;
        private readonly IWebHelper _webHelper;
        private readonly HttpClient _httpClient;
        private readonly ICustomerEntityService _customerEntityService;
        private readonly StripeAccountService _stripeAccountService;

        public PayoutService(IWorkContext workContext,
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
            _stripeAccountService = new StripeAccountService(_stripeConnectPaymentSettings.SecretKey);
        }

        bool ChangePayoutSettings(int customerId)
        {
            var domain = _customerEntityService.GetOrCreate(customerId);

            var accgetopts = new StripeRequestOptions();
            accgetopts.StripeConnectAccountId = domain.StripeUserId;

            var account = _stripeAccountService.Get(accgetopts);
            account.PayoutSchedule.DelayDays = 30;
            account.PayoutSchedule.Interval = "days";

            var updopts = new StripeAccountUpdateOptions();
            updopts.TransferScheduleDelayDays = "30"
            var postok = _stripeAccountService.Update(account.Id, updopts, accgetopts);

            var options = new StripePayoutCreateOptions();
            options.

            var stripepayout = _stripeAccountService.(options);
            return true;
        }
    }
}
