using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Localization;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Payments.PayPalMarketplace.Infrastructure
{
    /// <summary>
    /// Represents provider that provided basic routes
    /// </summary>
    public partial class RouteProvider : IRouteProvider
    {
        public const string ONBOARDING = "PayPalMarketPlaceOnBoarding";

        #region Methods

        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="routeBuilder">Route builder</param>
        public void RegisterRoutes(IRouteBuilder routeBuilder)
        {
            //routeBuilder.MapLocalizedRoute("Wishlist", "wishlist/{customerGuid?}",
            //    new { controller = "ShoppingCart", action = "Wishlist" });
            
            routeBuilder.MapLocalizedRoute(ONBOARDING, "PayPalMarketPlace/OnBoarding",
                new { controller = "OnBoarding", action = "Index" });
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a priority of route provider
        /// </summary>
        public int Priority
        {
            get { return 100; }
        }

        #endregion
    }
}
