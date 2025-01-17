﻿using Newtonsoft.Json;
using ASPMVCSignalRTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebMatrix.WebData;
using System.Web.Security;
using System.Net.Http.Headers;
using Microsoft.AspNet.SignalR;

namespace ASPMVCSignalRTest.APIControllers
{
    [APIVersionCheckActionFilter]
    public class ProductListsController : ApiController
    {
        ProductsDb m_tDb = new ProductsDb();
        IHubContext mProductsHubCtx;
        public class ProductListDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public ProductListsController()
        {
            mProductsHubCtx = GlobalHost.ConnectionManager.GetHubContext<ProductsHub>();
        }

        // GET api/productlists
        [System.Web.Mvc.Authorize]
        public HttpResponseMessage Get()
        {
            var lId = WebSecurity.CurrentUserId;
            var lProductLists = m_tDb.ProductLists.Where(aList => aList.Owner.UserId == WebSecurity.CurrentUserId).Select(aProductList => new ProductListDTO
            {
                Id = aProductList.Id,
                Name = aProductList.Name
            }).ToList();
            return this.Request.CreateResponse<List<ProductListDTO>>(HttpStatusCode.OK, lProductLists);


        }

        // POST api/productlists/create
        [HttpPost]
        [ActionName("create")]
        [System.Web.Mvc.Authorize]
        public HttpResponseMessage Create([FromBody]ProductListDTO aNewList)
        {
            var lOwner = m_tDb.UserProfiles.Find(WebSecurity.CurrentUserId);
            var lList = m_tDb.ProductLists.FirstOrDefault(aList => aList.Owner.UserId == WebSecurity.CurrentUserId && aList.Name == aNewList.Name);
            if (lList == null)
            {
                lList = new ProductList()
                {
                    Name = aNewList.Name,
                    Owner = lOwner,
                    Products = new List<ProductEntry>()
                };
                m_tDb.ProductLists.Add(lList);
                m_tDb.SaveChanges();
                var lConnectionIds = ProductsHub.GetConnectionsIdsOf(WebSecurity.CurrentUserName).ToArray();
                foreach (var lConnectionId in lConnectionIds)
                    mProductsHubCtx.Clients.Client(lConnectionId).OnServerEvent("ProductListCreated", new { Id = lList.Id, Name = lList.Name });
                return this.Request.CreateResponse<ProductListDTO>(HttpStatusCode.Created, new ProductListDTO { Id = lList.Id, Name = aNewList.Name });
            }
            return this.Request.CreateResponse<ProductListDTO>(HttpStatusCode.Conflict, new ProductListDTO { Id = lList.Id, Name = aNewList.Name });
        }

        // DELETE api/productsapi/delete/5
        [HttpDelete]
        [ActionName("delete")]
        [System.Web.Mvc.Authorize]
        public HttpResponseMessage Delete(int id)
        {
            var lUser = m_tDb.UserProfiles.Find(WebSecurity.CurrentUserId);
            var lList = m_tDb.ProductLists.FirstOrDefault(aList => aList.Owner.UserId == WebSecurity.CurrentUserId && aList.Id == id);
            if (lList == null)
                return this.Request.CreateResponse(HttpStatusCode.NotFound);

            lList.Products.Clear();
            m_tDb.ProductLists.Remove(lList);
            m_tDb.SaveChanges();
            var lConnectionIds = ProductsHub.GetConnectionsIdsOf(WebSecurity.CurrentUserName).ToArray();
            foreach (var lConnectionId in lConnectionIds)
                mProductsHubCtx.Clients.Client(lConnectionId).OnServerEvent("ProductListDeleted", new { Id = lList.Id, Name = lList.Name });
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
