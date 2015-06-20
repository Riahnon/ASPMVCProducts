﻿using Newtonsoft.Json;
using ASPMVCProducts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ASPMVCProducts.Controllers
{
    public class ProductsAPIController : ApiController
    {
        ProductsDb m_tDb = new ProductsDb();

       
        // GET api/productsapi
        public string Get()
        {
            var lProducts = m_tDb.Products.ToList();
            var lJSONData = JsonConvert.SerializeObject(lProducts);
            return lJSONData;
        }
        /*
        // GET api/productsapi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/productsapi
        public void Post([FromBody]string value)
        {
        }

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