using ASPMVCProducts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPMVCProducts.Controllers
{
	[Authorize]
	public class ProductListController : Controller
	{
		ProductsDb m_tDb = new ProductsDb();
		//
		// GET: /ProductList/1

		public ActionResult Index(int id = -1)
		{
			var lList = m_tDb.ProductLists.Where(aList => aList.Id == id).FirstOrDefault();
			
			return View(lList);
		}

		// POST: /Products/Delete/name=dasdsd
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int listid, int productid)
		{
			try
			{
				var lList = m_tDb.ProductLists.Where(aList => aList.Id == listid).FirstOrDefault();
				var lProduct = lList.Products.FirstOrDefault(aProd => aProd.Id == productid);
				if (lProduct != null)
				{
					lList.Products.Remove(lProduct);
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
