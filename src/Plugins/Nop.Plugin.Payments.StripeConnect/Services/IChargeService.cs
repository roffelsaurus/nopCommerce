using Nop.Services.Payments;

namespace Nop.Plugin.Payments.StripeConnect.Services
{
    public interface IChargeService
    {
        ProcessPaymentResult Charge(string token, decimal orderTotal, decimal orderSubTotal, int sellerCustomerId);
    }
}