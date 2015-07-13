using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Timers;

namespace ASPMVCProducts.Controllers
{
    public class HomeController : Controller
    {
        IHubContext mProductsHubCtx;
        private Timer m_tTimer;
        public HomeController()
        {
            m_tTimer = new Timer(3000);
            m_tTimer.Elapsed += (object source, ElapsedEventArgs e) => { _Ping(); };
            mProductsHubCtx = GlobalHost.ConnectionManager.GetHubContext<ProductsHub>();
        }
        public ActionResult Index()
        {
            m_tTimer.Start();
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();

        }
        private void _Ping()
        {
            mProductsHubCtx.Clients.All.OnServerEvent("asdasd", new { aaa=1, bbb="sdsa" });
        }
       
    }
}
