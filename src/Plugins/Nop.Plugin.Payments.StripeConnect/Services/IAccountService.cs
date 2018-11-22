namespace Nop.Plugin.Payments.StripeConnect.Services
{
    public interface IAccountService
    {
        bool ChangePayoutSettings(int customerId);
    }
}