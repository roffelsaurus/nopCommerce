using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nop.Core.Data;
using Nop.Plugin.Payments.StripeConnect.Domain;

namespace Nop.Plugin.Payments.StripeConnect.Services
{
    public class CustomerEntityService : ICustomerEntityService
    {
        private readonly IRepository<StripeCustomer> _repository;

        public CustomerEntityService(IRepository<StripeCustomer> repository)
        {
            _repository = repository;
        }

        public StripeCustomer GetOrCreate(int customerId)
        {
            var domainmodel = _repository.TableNoTracking
            .Where(i => i.CustomerId == customerId)
            .SingleOrDefault();

            if (domainmodel == null)
            {
                domainmodel = new StripeCustomer()
                {
                    CustomerId = customerId
                };
                _repository.Insert(domainmodel);
            }
            return domainmodel;
        }

        public void Update(StripeCustomer stripeCustomer)
        {
            _repository.Update(stripeCustomer);
        }
    }
}
