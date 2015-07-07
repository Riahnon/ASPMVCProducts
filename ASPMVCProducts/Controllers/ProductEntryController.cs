using ASPMVCProducts.Models;
using ASPMVCProducts.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace ASPMVCProducts.Controllers
{
	[Authorize]
	public class ProductEntryController : Controller
	{
		ProductsDb m_tDb = new ProductsDb();
        ProductList mList;
        public ProductEntryController()
        {

        }
		//
		// GET: /ProductEntry/1
		public ActionResult Index(int id = -1)
		{
            mList = m_tDb.ProductLists.Where(aList => aList.Id == id).FirstOrDefault();

            return View(mList.Products);
		}

        //
        // GET: /ProductEntry/Create
        public ActionResult Create(int id = -1)
        {
            if( id != -1)
                mList = m_tDb.ProductLists.Where(aList => aList.Id == id).FirstOrDefault();

            var lModel = new ProductEntryCreateViewModel();

            return View(lModel);
        }

        //
        // POST: /Products/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductEntryCreateViewModel aModel)
        {
            if( ModelState.IsValid && mList != null)
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
                    if (lOwner != null)
                    {
                        var lEntry = new ProductEntry()
                        {
                            Ammount = aModel.Ammount,
                            Comments = aModel.Comments,
                            Categories = new List<ProductCategory>(),
                            Product = lProduct,
                            List = mList,
                        };
                        mList.Products.Add(lEntry);
                        m_tDb.SaveChanges();
                    }
                }
                catch (DbUpdateException e)
                {

                }
            }
            return RedirectToAction("Index");
        }

		// POST: /productlist/delete/id=5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id)
		{
            if (mList != null)
            {
                try
                {
                    var lProductEntry = mList.Products.FirstOrDefault(aProd => aProd.Id == id);
                    if (lProductEntry != null)
                    {
                        mList.Products.Remove(lProductEntry);
                        m_tDb.SaveChanges();
                    }
                }
                catch
                {

                }
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
