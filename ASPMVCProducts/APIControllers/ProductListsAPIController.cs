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

namespace ASPMVCProducts.Controllers
{
	public class ProductListsAPIController : ApiController
	{
		ProductsDb m_tDb = new ProductsDb();

		public class ProductListDTO
		{
			public int Id { get; set; }
			public string Name { get; set; }
		}

        public class CreateProductListDTO
		{
			public string Name { get; set; }
		}
		// GET api/productsapi
		[Authorize]
		public IEnumerable<ProductListDTO> Get()
		{
			
			var lId = WebSecurity.CurrentUserId;
			var lProductLists = m_tDb.ProductLists.Select(aProductList => new ProductListDTO
			{
				Id = aProductList.Id,
				Name = aProductList.Name
			}).ToList();

			return lProductLists;
		}
		/*
		// GET api/productsapi/5
		public string Get(int id)
		{
				return "value";
		}*/

		// POST api/productsapi
        [HttpPost]
        [ActionName("create")]
        [Authorize]
        public HttpResponseMessage Create([FromBody]CreateProductListDTO aCreateList)
		{
            var lUser = m_tDb.UserProfiles.Find ( WebSecurity.CurrentUserId );
            var lList = m_tDb.ProductLists.FirstOrDefault(aList => aList.Owner.UserId == WebSecurity.CurrentUserId && aList.Name == aCreateList.Name);
            if (lList == null)
            {
                try
                {
                    lList = new ProductList()
                    {
                        Name = aCreateList.Name,
                        Owner = lUser,
                        Products = new List<ProductEntry>()
                    };
                    m_tDb.ProductLists.Add(lList);
                    m_tDb.SaveChanges();

                    return this.Request.CreateResponse<ProductListDTO>(HttpStatusCode.Created, new ProductListDTO { Id = lList.Id, Name = aCreateList.Name });
                }
                catch
                {
                }
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
                
            }
            return this.Request.CreateResponse<ProductListDTO>(HttpStatusCode.NotModified, new ProductListDTO { Id = lList.Id, Name = aCreateList.Name });
		}
        /*
		// PUT api/productsapi/5
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE api/productsapi/5
		public void Delete(int id)
		{
		}
		*/
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
