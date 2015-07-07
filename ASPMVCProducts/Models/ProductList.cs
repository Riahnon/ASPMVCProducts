using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASPMVCProducts.Models
{
	public class ProductList
	{
		[Key]
		public int Id { get; set; }
		[Key]
		public virtual UserProfile Owner { get; set; }
		[StringLength(256)]
		public string Name { get; set; }
		public virtual List<ProductEntry> Products { get; set; }
	}
}