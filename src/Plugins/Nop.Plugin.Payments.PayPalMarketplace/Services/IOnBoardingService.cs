using PayPal.v1.PartnerReferrals;

namespace Nop.Plugin.Payments.PayPalMarketplace.Services
{
    public interface IOnBoardingService
    {
        string GetActionUrl(PartnerReferral referral);
        PartnerReferral CreateNewReferral();
    }
}