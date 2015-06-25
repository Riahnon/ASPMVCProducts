using ASPMVCProducts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPMVCProducts.ViewModels.Products
{
	public class ProductCategoriesEditModel
	{
		public IEnumerable<ProductCategory> SelectedCategories { get; set; }
		public IEnumerable<ProductCategory> ExistingCategories { get; set; }
	}
}