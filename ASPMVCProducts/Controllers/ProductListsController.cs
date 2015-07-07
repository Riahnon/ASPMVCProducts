using ASPMVCProducts.Models;
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
	public class ProductListsController : Controller
	{
		ProductsDb m_tDb = new ProductsDb();
		//
		// GET: /ProductLists/

		public ActionResult Index()
		{
			var lLists = m_tDb.ProductLists.Where(aList => aList.Owner.UserId == WebSecurity.CurrentUserId).ToList();
			return View(lLists);
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
		[ValidateAntiForgeryToken]
		public ActionResult Create(FormCollection collection)
		{
			try
			{
				var lProductList = new ProductList()
				{
					Name = collection["Name"],
				};
				m_tDb.ProductLists.Add(lProductList);
				m_tDb.SaveChanges();
			}
			catch (DbUpdateException e)
			{

			}
			return RedirectToAction("Index");
		}

		// POST: /ProductLists/Delete/name=dasdsd
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id)
		{
			try
			{
				var lProductList = m_tDb.ProductLists.FirstOrDefault(aProdList => aProdList.Id == id);
				if (lProductList != null)
				{
					m_tDb.ProductLists.Remove(lProductList);
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
