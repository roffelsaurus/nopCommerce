namespace Nop.Plugin.Payments.StripeConnect.Services
{
    public interface IOnBoardingService
    {
        string NewOnboarding();
        bool ValidCSRFToken(string token);
    }
}