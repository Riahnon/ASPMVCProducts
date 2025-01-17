﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;

namespace ASPMVCSignalRTest
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            /*config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
            );*/

            config.Routes.MapHttpRoute(
                                            name: "ProductLists",
                                            routeTemplate: "api/ProductLists/{listid}/{action}/{id}",
                                            defaults: new { controller = "ProductEntries", action = RouteParameter.Optional, id = RouteParameter.Optional },
                                            constraints: new { listid = @"\d+" }
                                    );

            config.Routes.MapHttpRoute(
                            name: "DefaultApi",
                            routeTemplate: "api/{controller}/{action}/{id}",
                            defaults: new { action = RouteParameter.Optional, id = RouteParameter.Optional }
                        );


            //config.Filters.Add(new TokenValidationAttribute());
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();
        }
    }
}