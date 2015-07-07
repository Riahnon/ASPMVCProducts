using ASPMVCProducts.Models;
using ASPMVCProducts.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace ASPMVCProducts.Controllers
{
	[Authorize]
	public class ProductsController : Controller
	{
		ProductsDb m_tDb = new ProductsDb();
		//
		// GET: /Products/
		public ActionResult Index()
		{
			var lName = this.User.Identity.Name;
			var lModel = new ProductsViewModel()
			{
				Products = m_tDb.Products,
				ProductCategories = m_tDb.ProductCategories
			};

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
		[ValidateAntiForgeryToken]
		public ActionResult Create(FormCollection collection)
		{
			try
			{
				var lProduct = new Product()
				{
					Name = collection["Name"],
				};
				m_tDb.Products.Add(lProduct);
				m_tDb.SaveChanges();
			}
			catch (DbUpdateException e)
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
			if (lProduct != null)
			{
				return View(new ProductEditModel()
				{
					Product = lProduct,
					ProductCategories = m_tDb.ProductCategories
				});
			}

			return RedirectToAction("Index");
		}

		//
		// POST: /Products/Edit/5

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, FormCollection collection)
		{
			try
			{
				var lCategoryToken = "Category.";
				var lCategoryNames = collection.Keys.Cast<String>().Where(aKey => aKey.StartsWith(lCategoryToken)).Select(aKey => aKey.Remove(0, lCategoryToken.Length)).ToArray();

				//var lValueMap = collection.Keys.Select(aKey => new KeyValuePair<string, object>(aKey, collection[aKey])).ToArray();
				// TODO: Add update logic here
				var lProduct = m_tDb.Products.FirstOrDefault(aProduct => aProduct.Id == id);
				if (lProduct != null)
				{
					lProduct.Name = collection["Product.Name"];
					m_tDb.SaveChanges();
				}
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				return View();
			}
		}

		// POST: /Products/Delete/name=dasdsd
		[HttpPost]
		[ValidateAntiForgeryToken]
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
