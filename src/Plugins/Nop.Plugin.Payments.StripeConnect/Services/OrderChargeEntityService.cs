using Nop.Core.Data;
using Nop.Plugin.Payments.StripeConnect.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nop.Plugin.Payments.StripeConnect.Services
{
    public class OrderChargeEntityService : IOrderChargeEntityService
    {
        private readonly IRepository<StripeOrderCharge> _repository;

        public OrderChargeEntityService(IRepository<StripeOrderCharge> repository)
        {
            _repository = repository;
        }

        public StripeOrderCharge Get(Guid orderGuid)
        {
            var domainmodel = _repository.Table
            .Where(i => i.OrderGuid == orderGuid)
            .SingleOrDefault();

            return domainmodel;
        }

        public void Create(StripeOrderCharge stripeCharge)
        {
            _repository.Insert(stripeCharge);
        }
    }
}
