using ASPMVCProducts.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPMVCProducts.Controllers
{
    public class ProductsController : Controller
    {
        ProductsDb m_tDb = new ProductsDb();
        //
        // GET: /Products/

        public ActionResult Index()
        {
            var lModel = m_tDb.Products;
           
            return View(lModel);
        }

       

        //
        // GET: /Products/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Products/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var lProduct = new Product()
                {
                    Name= collection["Name"],
                    Description = collection["Name"]
                };
                m_tDb.Products.Add(lProduct);
                m_tDb.SaveChanges();
}
            catch ( DbUpdateException e )
            {
               
            }
            return RedirectToAction("Index");
        }

         //
        // GET: /Products/Edit/5

        public ActionResult WebAPI()
        {
            return View();
        }

        //
        // GET: /Products/Edit/5

        public ActionResult Edit(int id)
        {
            var lProduct = m_tDb.Products.FirstOrDefault(aProduct => aProduct.Id == id);
            if( lProduct != null )
                return View(lProduct);

            return RedirectToAction("Index");
        }

        //
        // POST: /Products/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var lProduct = m_tDb.Products.FirstOrDefault(aProduct => aProduct.Id == id);
                if (lProduct != null)
                {
                    lProduct.Name = collection["Name"];
                    lProduct.Description = collection["Description"];
                    m_tDb.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: /Products/Delete/name=dasdsd
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var lProduct = m_tDb.Products.FirstOrDefault(aProd => aProd.Id == id);
                if (lProduct != null)
                {
                    m_tDb.Products.Remove(lProduct);
                    m_tDb.SaveChanges();
                }
            }
            catch
            {
                
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
