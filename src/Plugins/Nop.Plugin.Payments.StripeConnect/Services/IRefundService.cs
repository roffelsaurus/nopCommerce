using Nop.Services.Payments;

namespace Nop.Plugin.Payments.StripeConnect.Services
{
    public interface IRefundService
    {
        RefundPaymentResult Refund(RefundPaymentRequest refundPaymentRequest);
    }
}