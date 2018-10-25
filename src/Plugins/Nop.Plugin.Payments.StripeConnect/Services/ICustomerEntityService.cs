using Nop.Plugin.Payments.StripeConnect.Domain;

namespace Nop.Plugin.Payments.StripeConnect.Services
{
    public interface ICustomerEntityService
    {
        StripeCustomer GetOrCreate(int customerId);
        void Update(StripeCustomer stripeCustomer);
    }
}