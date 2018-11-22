using Nop.Core;
using Nop.Services.Logging;
using Stripe;
using System.Net.Http;

namespace Nop.Plugin.Payments.StripeConnect.Services
{
    public class AccountService : IAccountService
    {
        private readonly IWorkContext _workContext;
        private readonly ILogger _logger;
        private readonly StripeConnectPaymentSettings _stripeConnectPaymentSettings;
        private readonly IWebHelper _webHelper;
        private readonly HttpClient _httpClient;
        private readonly ICustomerEntityService _customerEntityService;
        private readonly StripeAccountService _stripeAccountService;

        public AccountService(IWorkContext workContext,
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

        // Not used for MVP because Stripe standard accounts cant be changed this way, but code left for reference.
        public bool ChangePayoutSettings(int customerId)
        {
            var domain = _customerEntityService.GetOrCreate(customerId);
            
            var updopts = new StripeAccountUpdateOptions();
            updopts.TransferScheduleDelayDays = "30";
            updopts.TransferScheduleInterval = "daily";
            
            var postok = _stripeAccountService.Update(domain.StripeUserId, updopts);            

            return postok.PayoutSchedule.DelayDays == 30 && postok.PayoutSchedule.Interval == "daily";
        }
    }
}
