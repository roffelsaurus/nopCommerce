namespace Nop.Plugin.Payments.StripeConnect.Services
{
    public interface IOnBoardingService
    {
        bool GetNewAccessToken(string authCode, int customerId);
        string NewOnboarding();
        int? ConsumeCustomerMappingForCSRFToken(string token);
    }
}