using ASPMVCProducts.Models;
using ASPMVCProducts.ViewModels;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebMatrix.WebData;

namespace ASPMVCProducts.Controllers
{
    [System.Web.Mvc.Authorize]
    public class ProductListsController : Controller
    {
        ProductsDb m_tDb = new ProductsDb();
        IHubContext mProductsHubCtx;
        public ProductListsController()
        {
            mProductsHubCtx = GlobalHost.ConnectionManager.GetHubContext<ProductsHub>();
        }
        //
        // GET: /ProductLists/
        [OutputCache(NoStore = true, Duration = 0, Location = OutputCacheLocation.None, VaryByParam = "*")]
        public ActionResult Index()
        {
            var lLists = m_tDb.ProductLists.Where(aList => aList.Owner.UserId == WebSecurity.CurrentUserId).ToList();
            return View(lLists);
        }

        //
        // POST: /ProductLists/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductListCreateViewModel aProductList)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var lOwner = m_tDb.UserProfiles.Where(aUserProfie => aUserProfie.UserId == WebSecurity.CurrentUserId).FirstOrDefault();
                    if (lOwner != null)
                    {

                        var lProductList = new ProductList()
                        {
                            Name = aProductList.Name,
                            Owner = lOwner,
                            Products = new List<ProductEntry>()
                        };
                        m_tDb.ProductLists.Add(lProductList);
                        m_tDb.SaveChanges();
                        var lConnectionIds = ProductsHub.GetConnectionsIdsOf(WebSecurity.CurrentUserName).ToArray();
                        foreach (var lConnectionId in lConnectionIds)
                            mProductsHubCtx.Clients.Client(lConnectionId).OnServerEvent("ProductListCreated", new { Id = lProductList.Id, Name = lProductList.Name });
                    }
                }
                catch (DbUpdateException e)
                {
                    //No error notification implemented yet
                }
            }
            return RedirectToAction("Index");
        }

        // POST: /ProductLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id = 0)
        {
            try
            {
                var lProductList = m_tDb.ProductLists.Find(id);
                if (lProductList != null)
                {
                    lProductList.Products.Clear();
                    m_tDb.ProductLists.Remove(lProductList);
                    m_tDb.SaveChanges();
                    var lConnectionIds = ProductsHub.GetConnectionsIdsOf(WebSecurity.CurrentUserName).ToArray();
                    foreach (var lConnectionId in lConnectionIds)
                        mProductsHubCtx.Clients.Client(lConnectionId).OnServerEvent("ProductListDeleted", new { Id = lProductList.Id, Name = lProductList.Name });
                }
            }
            catch
            {
                //No error notification implemented yet
            }
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (m_tDb != null)
            {
                m_tDb.Dispose();
                m_tDb = null;
            }
            base.Dispose(disposing);
        }
    }
}
