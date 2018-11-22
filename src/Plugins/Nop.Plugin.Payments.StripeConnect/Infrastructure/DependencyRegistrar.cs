using Autofac;
using Autofac.Core;
using Nop.Core.Configuration;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;
using Nop.Plugin.Payments.StripeConnect.Services;
using Nop.Web.Framework.Infrastructure.Extensions;
using System.Net.Http;

namespace Nop.Plugin.Payments.StripeConnect.Infrastructure
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
            builder.RegisterInstance(new HttpClient()).SingleInstance();
            builder.RegisterType<OnBoardingService>().As<IOnBoardingService>().SingleInstance();
            builder.RegisterType<ChargeService>().As<IChargeService>();
            builder.RegisterType<AccountService>().As<IAccountService>();
            builder.RegisterType<RefundService>().As<IRefundService>();
            //data context
            builder.RegisterPluginDataContext<Data.StripeConnectObjectContext>("nop_object_context_stripeconnect");

            //override required repository with our custom context
            builder.RegisterType<EfRepository<Domain.StripeCustomer>>().As<IRepository<Domain.StripeCustomer>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_stripeconnect"))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<Domain.StripeOrderCharge>>().As<IRepository<Domain.StripeOrderCharge>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_stripeconnect"))
                .InstancePerLifetimeScope();
            
            builder.RegisterType<CustomerEntityService>().As<ICustomerEntityService>().InstancePerLifetimeScope();
            builder.RegisterType<OrderChargeEntityService>().As<IOrderChargeEntityService>().InstancePerLifetimeScope();
        }

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public int Order => 100;
    }
}