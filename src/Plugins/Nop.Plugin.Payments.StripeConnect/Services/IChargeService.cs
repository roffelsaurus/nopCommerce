using Nop.Services.Payments;
using Stripe;

namespace Nop.Plugin.Payments.StripeConnect.Services
{
    public interface IChargeService
    {
        ProcessPaymentResult Charge(string token, decimal orderSubTotal, int sellerCustomerId, ProcessPaymentRequest processPaymentRequest);
    }
}