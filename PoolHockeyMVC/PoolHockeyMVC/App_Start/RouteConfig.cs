using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PoolHockeyMVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

            // Improve SEO by stopping duplicate URL's due to case differences or trailing slashes.
            // See http://googlewebmastercentral.blogspot.co.uk/2010/04/to-slash-or-not-to-slash.html
            routes.AppendTrailingSlash = true;
            routes.LowercaseUrls = true;

            // IgnoreRoute - Tell the routing system to ignore certain routes for better performance.
            // Ignore .axd files.
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //// Ignore everything in the Content folder.
            //routes.IgnoreRoute("Content/{*pathInfo}");
            //// Ignore everything in the Scripts folder.
            //routes.IgnoreRoute("Scripts/{*pathInfo}");
            // Ignore the Forbidden.html file.
            routes.IgnoreRoute("Error/Forbidden.html");
            // Ignore the GatewayTimeout.html file.
            routes.IgnoreRoute("Error/GatewayTimeout.html");
            // Ignore the ServiceUnavailable.html file.
            routes.IgnoreRoute("Error/ServiceUnavailable.html");

            // Enable attribute routing.
            routes.MapMvcAttributeRoutes();

            // Keep it for Login/Register/Manage
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", Areas = "Common", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "login",
                url: "Account/Login"
            );

            routes.MapRoute(
                name: "register",
                url: "Account/Register"
            );

            routes.MapRoute(
                name: "logout",
                url: "Account/LogOff"
            );
        }
    }
}
