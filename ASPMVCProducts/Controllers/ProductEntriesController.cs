using ASPMVCProducts.Models;
using ASPMVCProducts.ViewModels;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebMatrix.WebData;

namespace ASPMVCProducts.Controllers
{
    [System.Web.Mvc.Authorize]
    public class ProductEntriesController : Controller
    {
        ProductsDb m_tDb = new ProductsDb();
        IHubContext mProductsHubCtx;
        public ProductEntriesController()
        {
            mProductsHubCtx = GlobalHost.ConnectionManager.GetHubContext<ProductsHub>();
        }
        //
        // GET: /ProductEntries/1
        [OutputCache(NoStore = true, Duration = 0, Location = OutputCacheLocation.None, VaryByParam = "*")]
        public ActionResult Index(int listid)
        {
            var lList = m_tDb.ProductLists.FirstOrDefault(aList => aList.Owner.UserId == WebSecurity.CurrentUserId && aList.Id == listid);
            if (lList != null)
            {
                return View(lList);
            }
            return new HttpNotFoundResult();
        }

        //
        // POST: /ProductEntries/Create/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductEntryCreateViewModel aModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var lProduct = m_tDb.Products.Where(aProduct => aProduct.Name == aModel.Name).FirstOrDefault();
                    if (lProduct == null)
                    {
                        lProduct = new Product() { Name = aModel.Name };
                        m_tDb.Products.Add(lProduct);
                    }
                    var lOwner = m_tDb.UserProfiles.Where(aUserProfie => aUserProfie.UserId == WebSecurity.CurrentUserId).FirstOrDefault();
                    var lList = m_tDb.ProductLists.FirstOrDefault(aList => aList.Id == aModel.ListId);
                    if (lOwner != null && lList != null)
                    {
                        var lEntry = lList.Products.Where(aProductEntry => aProductEntry.Product.Id == lProduct.Id).FirstOrDefault();
                        if (lEntry == null)
                        {
                            lEntry = new ProductEntry()
                            {
                                Amount = aModel.Amount,
                                Comments = aModel.Comments,
                                Product = lProduct,
                                List = lList,
                            };
                            lList.Products.Add(lEntry);
                            m_tDb.SaveChanges();
                            var lConnectionIds = ProductsHub.GetConnectionsIdsOf(WebSecurity.CurrentUserName).ToArray();
                            foreach (var lConnectionId in lConnectionIds)
                                mProductsHubCtx.Clients.Client(lConnectionId).OnServerEvent("ProductEntryCreated", new { ListId = lList.Id, Id = lEntry.Id, Name = lEntry.Product.Name, Amount = lEntry.Amount, Comments = lEntry.Comments });
                        }
                    }
                }
                catch (DbUpdateException e)
                {
                    //No error notification implemented yet
                }
            }
            return RedirectToAction("Index", new { listid = aModel.ListId });
        }

        // GET: /ProductEntries/1/Edit/2
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, Location = OutputCacheLocation.None, VaryByParam = "*")]
        public ActionResult Edit(int listid, int id)
        {
            var lList = m_tDb.ProductLists.FirstOrDefault(aList => aList.Owner.UserId == WebSecurity.CurrentUserId && aList.Id == listid);
            if (lList != null)
            {
                var lEntry = lList.Products.Find(aEntry => aEntry.Id == id);
                if (lEntry != null)
                {
                    var lViewModel = new ProductEntryEditViewModel()
                    {
                        ListId = listid,
                        EntryId = id,
                        Amount = lEntry.Amount,
                        Comments = lEntry.Comments
                    };

                    return View(lViewModel);
                }
            }
            return new HttpNotFoundResult();
        }

        // POST: /ProductLists/Edit/
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditConfirmed(ProductEntryEditViewModel aModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var lList = m_tDb.ProductLists.FirstOrDefault(aList => aList.Id == aModel.ListId);
                    if (lList != null)
                    {
                        var lEntry = lList.Products.Find(aEntry => aEntry.Id == aModel.EntryId);
                        if (lEntry != null)
                        {
                            lEntry.Amount = aModel.Amount;
                            lEntry.Comments = aModel.Comments;
                            m_tDb.SaveChanges();
                            var lConnectionIds = ProductsHub.GetConnectionsIdsOf(WebSecurity.CurrentUserName).ToArray();
                            foreach (var lConnectionId in lConnectionIds)
                                mProductsHubCtx.Clients.Client(lConnectionId).OnServerEvent("ProductEntryEdited", new { ListId = lList.Id, Id = lEntry.Id, Name = lEntry.Product.Name, Amount = lEntry.Amount, Comments = lEntry.Comments });
                        }
                    }
                }
                catch
                {
                    //No error notification implemented yet
                }
            }
            return RedirectToAction("Index", new { listid = aModel.ListId });
        }

        // POST: /ProductLists/4/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int listid, int id)
        {
            if (ModelState.IsValid)
            {
                var lList = m_tDb.ProductLists.FirstOrDefault(aList => aList.Id == listid);
                if (lList != null)
                {
                    var lEntry = lList.Products.Find(aEntry => aEntry.Id == id);
                    if (lEntry != null)
                    {
                        try
                        {
                            lList.Products.Remove(lEntry);
                            m_tDb.SaveChanges();
                            var lConnectionIds = ProductsHub.GetConnectionsIdsOf(WebSecurity.CurrentUserName).ToArray();
                            foreach (var lConnectionId in lConnectionIds)
                                mProductsHubCtx.Clients.Client(lConnectionId).OnServerEvent("ProductEntryDeleted", new { ListId = lList.Id, Id = lEntry.Id });
                        }
                        catch
                        {
                            //No error notification implemented yet
                        }
                    }
                }
            }
            return RedirectToAction("Index", new { listId = listid });
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
