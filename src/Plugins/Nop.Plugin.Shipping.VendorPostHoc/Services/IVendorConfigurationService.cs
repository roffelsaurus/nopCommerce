using Nop.Plugin.Shipping.VendorPostHoc.Domain;

namespace Nop.Plugin.Shipping.VendorPostHoc.Services
{
    public interface IVendorConfigurationService
    {
        VendorConfiguration GetForVendor(int vendorid);
        void Update(VendorConfiguration vendorConfiguration);
    }
}
