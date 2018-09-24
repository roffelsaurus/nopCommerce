using Nop.Core.Data;
using Nop.Plugin.Shipping.VendorPostHoc.Domain;
using System.Linq;

namespace Nop.Plugin.Shipping.VendorPostHoc.Services
{
    public class VendorConfigurationService : IVendorConfigurationService
    {
        private readonly IRepository<VendorConfiguration> _vendorconfigurationrepository;

        public VendorConfigurationService(
            IRepository<VendorConfiguration> vendorConfigurationRepository)
        {
            _vendorconfigurationrepository = vendorConfigurationRepository;
        }


        /// <summary>
        /// Gets for vendor id. Inserts if no entity is found.
        /// </summary>
        /// <param name="vendorid"></param>
        /// <returns></returns>
        public VendorConfiguration GetForVendor(int vendorid)
        {
            var domainmodel = _vendorconfigurationrepository.TableNoTracking
        .Where(i => i.VendorId == vendorid)
        .SingleOrDefault();

            if (domainmodel == null)
            {
                domainmodel = new VendorConfiguration()
                {
                    VendorId = vendorid,
                    ShippingCost = 0m
                };
                _vendorconfigurationrepository.Insert(domainmodel);
            }
            return domainmodel;
        }

        public void Update(VendorConfiguration vendorConfiguration)
        {
            _vendorconfigurationrepository.Update(vendorConfiguration);
        }

}
}
