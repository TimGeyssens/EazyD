using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EazyD
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "EazyD",
                url: "umbraco/backoffice/Plugins/EazyD/{action}/{id}",
                defaults: new { controller = "EazyD", action = "Dialog", id = UrlParameter.Optional }
            );
        }
    }
}