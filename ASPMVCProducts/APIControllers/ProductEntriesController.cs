using ASPMVCProducts.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebMatrix.WebData;

namespace ASPMVCProducts.APIControllers
{
    [APIVersionCheckActionFilter]
    public class ProductEntriesController : ApiController
    {
        ProductsDb m_tDb = new ProductsDb();
        IHubContext mProductsHubCtx;

        public class ProductEntryDTO
        {
            public int Id { get; set; }
            public string ProductName { get; set; }
            public int Amount { get; set; }
            public string Comments { get; set; }
        }

        public ProductEntriesController()
        {
            mProductsHubCtx = GlobalHost.ConnectionManager.GetHubContext<ProductsHub>();
        }

        // GET api/productlists/5/productentries
        [System.Web.Mvc.Authorize]
        public HttpResponseMessage Get(int listid)
        {
            var lId = WebSecurity.CurrentUserId;
            var lProductList = m_tDb.ProductLists.FirstOrDefault(aList => aList.Owner.UserId == WebSecurity.CurrentUserId && aList.Id == listid);
            if (lProductList == null)
                return this.Request.CreateResponse(HttpStatusCode.NotFound);

            var lEntries = lProductList.Products.Select(aProductEntry => new ProductEntryDTO()
            {
                Id = aProductEntry.Id,
                ProductName = aProductEntry.Product.Name,
                Amount = aProductEntry.Amount,
                Comments = aProductEntry.Comments
            }).ToList();
            return this.Request.CreateResponse<List<ProductEntryDTO>>(HttpStatusCode.OK, lEntries);
        }

        // POST api/productlists/5/create
        [HttpPost]
        [ActionName("create")]
        [System.Web.Mvc.Authorize]
        public HttpResponseMessage Create(int listid, [FromBody]ProductEntryDTO aEntry)
        {
            var lUser = m_tDb.UserProfiles.Find(WebSecurity.CurrentUserId);
            var lList = m_tDb.ProductLists.FirstOrDefault(aList => aList.Owner.UserId == WebSecurity.CurrentUserId && aList.Id == listid);
            if (lList == null)
                return this.Request.CreateResponse(HttpStatusCode.NotFound);

            var lProduct = m_tDb.Products.Where(aProduct => aProduct.Name == aEntry.ProductName).FirstOrDefault();
            if (lProduct == null)
            {
                lProduct = new Product() { Name = aEntry.ProductName };
                m_tDb.Products.Add(lProduct);
            }
            var lEntry = lList.Products.Where(aProductEntry => aProductEntry.Product.Id == lProduct.Id).FirstOrDefault();
            if (lEntry == null)
            {
                lEntry = new ProductEntry()
                {
                    Product = lProduct,
                    List = lList,
                };
                lList.Products.Add(lEntry);
                m_tDb.SaveChanges();
                var lConnectionIds = ProductsHub.GetConnectionsIdsOf(WebSecurity.CurrentUserName).ToArray();
                foreach (var lConnectionId in lConnectionIds)
                    mProductsHubCtx.Clients.Client(lConnectionId).OnServerEvent("ProductEntryCreated", new { ListId = lList.Id, Id = lEntry.Id, Name = lEntry.Product.Name, Amount = lEntry.Amount, Comments = lEntry.Comments });
                return this.Request.CreateResponse(HttpStatusCode.Created,
                        new ProductEntryDTO() { Id = lEntry.Id, ProductName = lProduct.Name, Amount = lEntry.Amount, Comments = lEntry.Comments });
            }
            return this.Request.CreateResponse<ProductEntryDTO>(HttpStatusCode.Conflict,
                    new ProductEntryDTO() { Id = lEntry.Id, ProductName = lProduct.Name, Amount = lEntry.Amount, Comments = lEntry.Comments });

        }

        // POST api/productlists/5/edit/4
        [HttpPut]
        [ActionName("edit")]
        [System.Web.Mvc.Authorize]
        public HttpResponseMessage Edit(int listid, int id, [FromBody]ProductEntryDTO aEntry)
        {
            var lUser = m_tDb.UserProfiles.Find(WebSecurity.CurrentUserId);
            var lList = m_tDb.ProductLists.FirstOrDefault(aList => aList.Owner.UserId == WebSecurity.CurrentUserId && aList.Id == listid);
            if (lList == null)
                return this.Request.CreateResponse(HttpStatusCode.NotFound);

            var lEntry = lList.Products.FirstOrDefault(aProduct => aProduct.Id == id);
            if (lEntry == null)
                return this.Request.CreateResponse(HttpStatusCode.NotFound);

            lEntry.Amount = aEntry.Amount;
            lEntry.Comments = aEntry.Comments;
            m_tDb.SaveChanges();
            var lConnectionIds = ProductsHub.GetConnectionsIdsOf(WebSecurity.CurrentUserName).ToArray();
            foreach (var lConnectionId in lConnectionIds)
                mProductsHubCtx.Clients.Client(lConnectionId).OnServerEvent("ProductEntryEdited", new { ListId = lList.Id, Id = lEntry.Id, Name = lEntry.Product.Name, Amount = lEntry.Amount, Comments = lEntry.Comments });
            return this.Request.CreateResponse(HttpStatusCode.OK);

        }


        // DELETE api/productlists/5/delete/5
        [HttpDelete]
        [ActionName("delete")]
        [System.Web.Mvc.Authorize]
        public HttpResponseMessage Delete(int listid, int id)
        {
            var lUser = m_tDb.UserProfiles.Find(WebSecurity.CurrentUserId);
            var lList = m_tDb.ProductLists.FirstOrDefault(aList => aList.Owner.UserId == WebSecurity.CurrentUserId && aList.Id == listid);
            if (lList == null)
                return this.Request.CreateResponse(HttpStatusCode.NotFound);

            var lEntry = lList.Products.FirstOrDefault(aProduct => aProduct.Id == id);
            if (lEntry == null)
                return this.Request.CreateResponse(HttpStatusCode.NotFound);


            lList.Products.Remove(lEntry);
            m_tDb.SaveChanges();

            var lConnectionIds = ProductsHub.GetConnectionsIdsOf(WebSecurity.CurrentUserName).ToArray();
            foreach (var lConnectionId in lConnectionIds)
                mProductsHubCtx.Clients.Client(lConnectionId).OnServerEvent("ProductEntryDeleted", new { ListId = lList.Id, Id = lEntry.Id });
            return this.Request.CreateResponse(HttpStatusCode.OK);
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
