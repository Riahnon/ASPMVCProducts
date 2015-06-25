using ASPMVCProducts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPMVCProducts.ViewModels.Products
{
	public class ProductEditModel
	{
		public Product Product { get; set; }
		public IEnumerable<ProductCategory> ProductCategories { get; set; }
	}
}