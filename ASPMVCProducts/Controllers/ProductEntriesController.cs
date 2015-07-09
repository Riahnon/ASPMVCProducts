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
	public class ProductEntriesController : Controller
	{
		ProductsDb m_tDb = new ProductsDb();
		public ProductEntriesController()
		{

		}
		//
		// GET: /ProductEntries/1
		public ActionResult Index(int listid)
		{
			var lList = m_tDb.ProductLists.FirstOrDefault( aList=> aList.Id == listid);
			if (lList != null)
			{
				return View(lList);
			}
			return new HttpNotFoundResult();
		}

		//
		// GET: /ProductEntries/Create/1
		public ActionResult Create( int listid )
		{
			var lList = m_tDb.ProductLists.FirstOrDefault( aList=> aList.Id == listid);
			if (lList != null)
			{
				var lModel = new ProductEntryCreateViewModel() { ListId = listid };
				return View(lModel);
			}
			return new HttpNotFoundResult();
		}

		//
		// POST: /ProductEntries/Create/1
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ProductEntryCreateViewModel aModel)
		{
			if (ModelState.IsValid)
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
					var lList = m_tDb.ProductLists.FirstOrDefault(aList => aList.Id == aModel.ListId);
					if (lOwner != null && lList != null)
					{
						var lEntry = new ProductEntry()
						{
							Ammount = aModel.Ammount,
							Comments = aModel.Comments,
							Categories = new List<ProductCategory>(),
							Product = lProduct,
							List = lList,
						};
						lList.Products.Add(lEntry);
						m_tDb.SaveChanges();
					}
				}
				catch (DbUpdateException e)
				{

				}
			}
			return RedirectToAction("Index", new { listid = aModel.ListId });
		}

		// GET: /ProductEntries/Edit/1&2
		[HttpGet]
		public ActionResult Edit(int listid, int entryid)
		{
			var lList = m_tDb.ProductLists.FirstOrDefault(aList => aList.Id == listid);
			if (lList != null)
			{
				var lEntry = lList.Products.Find(aEntry => aEntry.Id == entryid);
				if (lEntry != null)
				{
					var lViewModel = new ProductEntryEditViewModel()
					{
						ListId = listid,
						EntryId = entryid,
						Ammount = lEntry.Ammount,
						Checked = lEntry.Checked,
						Comments = lEntry.Comments
					};
					return View(lViewModel);
				}
			}
			return new HttpNotFoundResult();
		}

		// POST: /ProductLists/Edit/
		[HttpPost, ActionName("Edit")]
		[ValidateAntiForgeryToken]
		public ActionResult EditConfirmed(ProductEntryEditViewModel aModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var lList = m_tDb.ProductLists.FirstOrDefault(aList => aList.Id == aModel.ListId);
					if (lList != null)
					{
						var lEntry = lList.Products.Find(aEntry => aEntry.Id == aModel.EntryId);
						if (lEntry != null)
						{

							lEntry.Checked = aModel.Checked;
							lEntry.Ammount = aModel.Ammount;
							lEntry.Comments = aModel.Comments;
							m_tDb.SaveChanges();
						}
					}
				}
				catch
				{

				}
			}
			return RedirectToAction("Index", new { listid = aModel.ListId });
		}

		// GET: /ProductEntries/Delete/1&2
		[HttpGet]
		public ActionResult Delete(int listid, int entryid)
		{
			var lList = m_tDb.ProductLists.FirstOrDefault( aList=> aList.Id == listid);
			if (lList != null)
			{
				var lEntry = lList.Products.Find(aEntry => aEntry.Id == entryid);
				if (lEntry != null)
				{
					var lViewModel = new ProductEntryDeleteViewModel()
					{
						ListId = listid,
						EntryId = entryid,
						Name = lEntry.Product.Name
					};
					return View(lViewModel);
				}
			}
			return new HttpNotFoundResult();
		}

		// POST: /ProductLists/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(ProductEntryDeleteViewModel aModel)
		{
			if (ModelState.IsValid)
			{
				var lList = m_tDb.ProductLists.FirstOrDefault(aList => aList.Id == aModel.ListId);
				if (lList != null)
				{
					var lEntry = lList.Products.Find(aEntry => aEntry.Id == aModel.EntryId);
					if (lEntry != null)
					{
						try
						{
							lList.Products.Remove(lEntry);
							m_tDb.SaveChanges();
						}
						catch
						{

						}
					}
				}
			}
			return RedirectToAction("Index", new { listId = aModel.ListId });
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
