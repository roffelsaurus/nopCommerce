using Nop.Plugin.Payments.StripeConnect.Domain;
using System;

namespace Nop.Plugin.Payments.StripeConnect.Services
{
    public interface IOrderChargeEntityService
    {
        void Create(StripeOrderCharge stripeCharge);
        StripeOrderCharge Get(Guid orderGuid);
    }
}