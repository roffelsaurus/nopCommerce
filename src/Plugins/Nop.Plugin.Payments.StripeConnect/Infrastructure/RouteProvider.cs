using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Localization;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Payments.StripeConnect.Infrastructure
{
    /// <summary>
    /// Represents provider that provided basic routes
    /// </summary>
    public partial class RouteProvider : IRouteProvider
    {
        public const string ONBOARDING_NAME = "StripeConnectOnBoarding";
        public const string ONBOARDING_ROUTE = "StripeConnect/OnBoarding";
        public const string ONBOARDING_REDIRECT_NAME = "StripeConnectOnBoardingRedirect";
        public const string ONBOARDING_REDIRECT_ROUTE = "StripeConnect/OnBoarding/Redirect";
        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="routeBuilder">Route builder</param>
        public void RegisterRoutes(IRouteBuilder routeBuilder)
        {
            //routeBuilder.MapLocalizedRoute("Wishlist", "wishlist/{customerGuid?}",
            //    new { controller = "ShoppingCart", action = "Wishlist" });
            
            routeBuilder.MapRoute(ONBOARDING_NAME, ONBOARDING_ROUTE,
                new { controller = "OnBoarding", action = "Index" });

            routeBuilder.MapRoute(ONBOARDING_REDIRECT_NAME, ONBOARDING_REDIRECT_ROUTE,
                new { controller = "OnBoarding", action = "Redirect" });
        }
        
        

        /// <summary>
        /// Gets a priority of route provider
        /// </summary>
        public int Priority
        {
            get { return -1; }
        }
        
    }
}
