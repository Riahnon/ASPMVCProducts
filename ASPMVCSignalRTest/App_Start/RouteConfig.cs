﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;

namespace ASPMVCSignalRTest
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                            name: "ProductEntries",
                            url: "ProductLists/{listid}/{action}/{id}",
                            defaults: new { controller = "ProductEntries", action = "Index", id = UrlParameter.Optional },
                            constraints: new { listid = @"\d+" }
            );

            routes.MapRoute(
                    name: "Default",
                    url: "{controller}/{action}/{id}",
                    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}