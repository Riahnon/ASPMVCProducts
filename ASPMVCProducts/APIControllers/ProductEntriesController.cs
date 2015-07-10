using ASPMVCProducts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebMatrix.WebData;

namespace ASPMVCProducts.APIControllers
{
    public class ProductEntriesController : ApiController
    {
			ProductsDb m_tDb = new ProductsDb();

			public class ProductEntryDTO
			{
				public int Id { get; set; }
				public string ProductName { get; set; }
				public bool Checked { get; set; }
				public int Ammount { get; set; }
				public string Comments { get; set; }
			}

			public class CreateProductEntryDTO
			{
				public string ProductName { get; set; }
			}


			// GET api/productlists/5/productentries
			[Authorize]
			public HttpResponseMessage Get(int listid)
			{
				var lId = WebSecurity.CurrentUserId;
				var lProductList = m_tDb.ProductLists.FirstOrDefault (aProductList => aProductList.Id == listid );
				if(lProductList == null)
					return this.Request.CreateResponse(HttpStatusCode.NotFound);

				var lEntries = lProductList.Products.Select ( aProductEntry => new ProductEntryDTO ()
				{
					Id = aProductEntry.Id,
					ProductName = aProductEntry.Product.Name,
					Ammount = aProductEntry.Ammount,
					Checked = aProductEntry.Checked,
					Comments = aProductEntry.Comments
				}).ToList();
				return this.Request.CreateResponse<List<ProductEntryDTO>>(HttpStatusCode.OK, lEntries);
			}

			// POST api/productlists/5/create
			[HttpPost]
			[ActionName("create")]
			[Authorize]
			public HttpResponseMessage Create(int listid, [FromBody]CreateProductEntryDTO aCreateEntry)
			{
				var lUser = m_tDb.UserProfiles.Find(WebSecurity.CurrentUserId);
				var lList = m_tDb.ProductLists.FirstOrDefault(aList => aList.Owner.UserId == WebSecurity.CurrentUserId && aList.Id == listid);
				if (lList == null)
					return this.Request.CreateResponse(HttpStatusCode.NotFound);

				try
				{
					var lProduct = m_tDb.Products.Where(aProduct => aProduct.Name == aCreateEntry.ProductName).FirstOrDefault();
					if (lProduct == null)
					{
						lProduct = new Product() { Name = aCreateEntry.ProductName };
						m_tDb.Products.Add(lProduct);
					}
					var lEntry = new ProductEntry()
					{
						Categories = new List<ProductCategory>(),
						Product = lProduct,
						List = lList,
					};
					lList.Products.Add(lEntry);
					m_tDb.SaveChanges();
					return this.Request.CreateResponse(HttpStatusCode.Created);
				}
				catch
				{
					return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
				}
			}

			// DELETE api/productlists/5/delete/5
			[HttpDelete]
			[ActionName("delete")]
			[Authorize]
			public HttpResponseMessage Delete(int listid, int id)
			{
				var lUser = m_tDb.UserProfiles.Find(WebSecurity.CurrentUserId);
				var lList = m_tDb.ProductLists.FirstOrDefault(aList => aList.Owner.UserId == WebSecurity.CurrentUserId && aList.Id == listid);
				if (lList == null)
					return this.Request.CreateResponse(HttpStatusCode.NotFound);

				var lEntry = lList.Products.FirstOrDefault(aProduct => aProduct.Id == id);
				if (lEntry == null)
					return this.Request.CreateResponse(HttpStatusCode.NotFound);

				try
				{
					lList.Products.Remove(lEntry);
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
