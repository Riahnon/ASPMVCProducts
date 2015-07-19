using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPMVCProducts
{
    [assembly: OwinStartup(typeof(ASPMVCProducts.Startup))]
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}