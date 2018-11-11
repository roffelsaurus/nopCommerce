using Autofac;
using Autofac.Core;
using Nop.Core.Configuration;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;
using Nop.Plugin.Shipping.VendorPostHoc.Domain;
using Nop.Plugin.Shipping.VendorPostHoc.Services;
using Nop.Services.Orders;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Framework.Infrastructure.Extensions;

namespace Nop.Plugin.Shipping.VendorPostHoc.Infrastructure
{
    /// <summary>
    /// Dependency registrar
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            //builder.RegisterType<ShippingByWeightByTotalService>().As<IShippingByWeightByTotalService>().InstancePerLifetimeScope();

            ////data context
            //builder.RegisterPluginDataContext<ShippingByWeightByTotalObjectContext>("nop_object_context_shipping_weight_total_zip");

            ////override required repository with our custom context
            //builder.RegisterType<EfRepository<ShippingByWeightByTotalRecord>>().As<IRepository<ShippingByWeightByTotalRecord>>()
            //    .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_shipping_weight_total_zip"))
            //    .InstancePerLifetimeScope();
            // builder.RegisterType<VendorTransparentOrderModelFactory>().As<IOrderModelFactory>().InstancePerLifetimeScope();


            //data context
            builder.RegisterPluginDataContext<Data.VendorPostHocObjectContext>("nop_object_context_vendorposthoc");

            //override required repository with our custom context
            builder.RegisterType<EfRepository<VendorConfiguration>>().As<IRepository<VendorConfiguration>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_vendorposthoc"))
                .InstancePerLifetimeScope();
            builder.RegisterType<VendorConfigurationService>().As<IVendorConfigurationService>().InstancePerLifetimeScope();
            builder.RegisterType<RetradeShoppingCartService>().As<IShoppingCartService>().InstancePerLifetimeScope();
        }

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public int Order => 100;
    }
}