using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ASPMVCProducts.Models
{
	public class ProductEntry
	{
		[Key]
		public int Id { get; set; }
		[Key]
		public ProductList List { get; set; }
		public virtual Product Product { get; set; }
		public bool Checked { get; set; }
		public int Ammount { get; set; }
		public virtual List<ProductCategory> Categories { get; set; }
		public string Comments { get; set; }

	}
}