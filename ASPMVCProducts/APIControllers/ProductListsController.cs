using Newtonsoft.Json;
using ASPMVCProducts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebMatrix.WebData;
using System.Web.Security;
using System.Net.Http.Headers;

namespace ASPMVCProducts.APIControllers
{
	public class ProductListsController : ApiController
	{
		ProductsDb m_tDb = new ProductsDb();

		public class ProductListDTO
		{
			public int Id { get; set; }
			public string Name { get; set; }
		}

		// GET api/productlists
		[Authorize]
		public HttpResponseMessage Get()
		{
			try
			{
				var lId = WebSecurity.CurrentUserId;
				var lProductLists = m_tDb.ProductLists.Select(aProductList => new ProductListDTO
				{
					Id = aProductList.Id,
					Name = aProductList.Name
				}).ToList();

				return this.Request.CreateResponse<List<ProductListDTO>>(HttpStatusCode.OK, lProductLists);
			}
			catch
			{
				return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
			}
		}

		// POST api/productlists/create
		[HttpPost]
		[ActionName("create")]
		[Authorize]
		public HttpResponseMessage Create([FromBody]ProductListDTO aNewList)
		{
			var lUser = m_tDb.UserProfiles.Find(WebSecurity.CurrentUserId);
            var lList = m_tDb.ProductLists.FirstOrDefault(aList => aList.Owner.UserId == WebSecurity.CurrentUserId && aList.Name == aNewList.Name);
			if (lList == null)
			{
				try
				{
					lList = new ProductList()
					{
                        Name = aNewList.Name,
						Owner = lUser,
						Products = new List<ProductEntry>()
					};
					m_tDb.ProductLists.Add(lList);
					m_tDb.SaveChanges();

                    return this.Request.CreateResponse<ProductListDTO>(HttpStatusCode.Created, new ProductListDTO { Id = lList.Id, Name = aNewList.Name });
				}
				catch
				{
					return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
				}
			}
            return this.Request.CreateResponse<ProductListDTO>(HttpStatusCode.Conflict, new ProductListDTO { Id = lList.Id, Name = aNewList.Name });
		}

		// DELETE api/productsapi/delete/5
		[HttpDelete]
		[ActionName("delete")]
		[Authorize]
		public HttpResponseMessage Delete(int id)
		{
			var lUser = m_tDb.UserProfiles.Find(WebSecurity.CurrentUserId);
			var lList = m_tDb.ProductLists.FirstOrDefault(aList => aList.Owner.UserId == WebSecurity.CurrentUserId && aList.Id == id);
			if (lList == null)
				return this.Request.CreateResponse(HttpStatusCode.NotFound);
			try
			{
				lList.Products.Clear();
				m_tDb.ProductLists.Remove(lList);
				m_tDb.SaveChanges();
				return this.Request.CreateResponse(HttpStatusCode.OK);
			}
			catch
			{
				return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
			}
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
