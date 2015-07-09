using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ASPMVCProducts.Controllers
{
    public class ProductCategoriesAPIController : ApiController
    {
        // GET api/productcategoriesapi
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/productcategoriesapi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/productcategoriesapi
        public void Post([FromBody]string value)
        {
        }

        // PUT api/productcategoriesapi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/productcategoriesapi/5
        public void Delete(int id)
        {
        }
    }
}
